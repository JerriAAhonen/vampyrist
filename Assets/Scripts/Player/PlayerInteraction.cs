using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField] private Transform carryParent;
	[SerializeField] private LayerMask groundedRuneMask;
	[SerializeField] private LayerMask carryingRuneMask;
	[SerializeField] private LayerMask runeSlotMask;

	private PlayerController controller;
	private PlayerMovement movement;
	private float interactionRadius = 1f;
	private Vector3 defaultCarryParentPos;
	private Rune carryingRune;

	private IHighlightable highlightedRune;
	private IHighlightable highlightedSlot;

	public bool IsCarrying => carryingRune != null;

	public void Init(PlayerController controller, PlayerMovement movement)
	{
		this.controller = controller;
		this.movement = movement;
	}

	private void Start()
	{
		InputController.Instance.Interact += OnInteract;
		defaultCarryParentPos = carryParent.localPosition;
	}

	private void Update()
	{
		// Carry parent
		if (movement.MovementDir.x >= 0)
			carryParent.transform.localPosition = defaultCarryParentPos;
		else
			carryParent.transform.localPosition = new Vector3(-defaultCarryParentPos.x, defaultCarryParentPos.y, defaultCarryParentPos.z);

		// Grounded runes
		var hits = Physics2D.CircleCastAll(transform.position, interactionRadius, transform.forward, 0f, groundedRuneMask);
		var highlightables = new List<IHighlightable>(hits.Length);
		foreach (var hit in hits)
			highlightables.Add(hit.transform.GetComponent<IHighlightable>());
		HighlightClosest(highlightables, ref highlightedRune);

		// Rune slots
		// Cannot highlight if not carrying any
		if (!carryingRune)
			return;

		hits = Physics2D.CircleCastAll(transform.position, interactionRadius, transform.forward, 0f, runeSlotMask);
		highlightables = new List<IHighlightable>(hits.Length);
		foreach (var hit in hits)
			highlightables.Add(hit.transform.GetComponent<IHighlightable>());
		HighlightClosest(highlightables, ref highlightedSlot);
	}

	private void OnDestroy()
	{
		if (InputController.Instance)
			InputController.Instance.Interact -= OnInteract;
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;
		if (!DebugManager.Instance.ShowDebugOverlay) return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, interactionRadius);
	}

	public void OnDie()
	{
		if (InputController.Instance)
			InputController.Instance.Interact -= OnInteract;
	}

	private void OnInteract()
	{
		var hits = Physics2D.CircleCastAll(transform.position, interactionRadius, transform.forward).ToList();

		foreach (var hit in hits)
		{
			// Pickup rune
			var rune = hit.transform.GetComponent<Rune>();
            if (rune && !carryingRune)
            {
				if (!rune.CanPickup)
					continue;

				Pickup(rune);
				return;
            }

			// Set rune into slot
			var slot = hit.transform.GetComponent<RuneSlot>();
			if (slot && carryingRune)
			{
				if (slot.Locked)
					continue;

				InsertInSlot(slot);
				return;
			}
        }

		// Throw rune
		if (carryingRune)
		{
			Throw();
			return;
		}

		void Pickup(Rune rune)
		{
			carryingRune = rune;
			rune.Pickup(carryParent);
		}

		void InsertInSlot(RuneSlot slot)
		{
			var oldRune = carryingRune;
			
			var runeInSlot = slot.InsertedRune;
			if (runeInSlot)
				Pickup(runeInSlot);
			
			oldRune.InsertIntoSlot(slot);
			carryingRune = runeInSlot;
		}

		void Throw()
		{
			carryingRune.Throw(movement.MovementDir);
			carryingRune = null;
		}
	}

	private void HighlightClosest(List<IHighlightable> hits, ref IHighlightable current)
	{
		// If we hit any runes on the ground
		if (hits.Count > 0)
		{
			var dist = 999f;
			IHighlightable closest = null;
			foreach (var hit in hits)
			{
				// Calculate distance to this rune
				var newDist = Vector3.Distance(hit.Position, transform.position);
				if (newDist < dist)
				{
					// If it's shorter than the previous, save it as the new closest one
					dist = newDist;
					closest = hit;
				}
			}

			// If we indeed found a rune close by, and it's not the highlighted rune (which it shouldn't)
			if (closest != null && closest != current)
			{
				// If we have a highlighted rune, deactivate it
				current?.ActivateHighlight(false);

				// Activate new highlighted rune
				current = closest;
				current.ActivateHighlight(true);
			}
		}
		// We didn't hit anything, deactivate the old one if there is one
		else
		{
			current?.ActivateHighlight(false);
			current = null;
		}
	}
}

public interface IHighlightable
{
	public Vector3 Position { get; }
	public void ActivateHighlight(bool activate);
}
