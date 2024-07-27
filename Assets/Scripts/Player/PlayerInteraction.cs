using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField] private Transform carryParent;
	[SerializeField] private LayerMask groundedRuneMask;
	[SerializeField] private LayerMask carryingRuneMask;

	private PlayerController controller;
	private PlayerMovement movement;
	private float interactionRadius = 0.5f;
	private Rune carryingRune;

	public bool IsCarrying => carryingRune != null;

	public void Init(PlayerController controller, PlayerMovement movement)
	{
		this.controller = controller;
		this.movement = movement;
	}

	private void Start()
	{
		InputController.Instance.Interact += OnInteract;
	}

	private void Update()
	{
		var hits = Physics2D.CircleCastAll(transform.position, interactionRadius, transform.forward, groundedRuneMask).ToList();

		if (hits.Count > 0)
		{
			// TODO Show tooltip
		}
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
			Debug.Log("Pickup");

			carryingRune = rune;
			rune.Pickup();
			rune.transform.SetParent(carryParent);
			rune.transform.localPosition = Vector3.zero;

			// TODO Move to Rune.cs
			var carryingRuneLayer = Mathf.RoundToInt(Mathf.Log(carryingRuneMask.value, 2));
			rune.gameObject.layer = carryingRuneLayer;
		}

		void InsertInSlot(RuneSlot slot)
		{
			Debug.Log("InsertInSlot");

			var runeInSlot = slot.InsertedRune;
			carryingRune.transform.SetParent(null);

			// TODO Move to Rune.cs
			var groundedRuneLayer = Mathf.RoundToInt(Mathf.Log(groundedRuneMask.value, 2));
			carryingRune.gameObject.layer = groundedRuneLayer;

			carryingRune.InsertIntoSlot(slot);
			carryingRune = null;

			if (runeInSlot)
				Pickup(runeInSlot);
		}

		void Throw()
		{
			Debug.Log("Throw");

			carryingRune.transform.SetParent(null);
			carryingRune.Throw(movement.MovementDir);

			var groundedRuneLayer = Mathf.RoundToInt(Mathf.Log(groundedRuneMask.value, 2));
			carryingRune.gameObject.layer = groundedRuneLayer;
			carryingRune = null;
		}
	}
}
