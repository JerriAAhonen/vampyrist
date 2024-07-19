using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField] private Transform carryParent;
	[SerializeField] private LayerMask groundedRuneMask;
	[SerializeField] private LayerMask carryingRuneMask;

	private readonly RaycastHit2D[] hits = new RaycastHit2D[20];

	private PlayerMovement movement;
	private float interactionRadius = 0.5f;
	private Rune carryingRune;

	public bool IsCarrying => carryingRune != null;

	private void Awake()
	{
		movement = GetComponent<PlayerMovement>();
	}

	private void Start()
	{
		InputController.Instance.Interact += OnInteract;
	}

	private void Update()
	{
		var hitCount = Physics2D.CircleCastNonAlloc(transform.position, interactionRadius, transform.forward, hits, groundedRuneMask);

		if (hitCount > 0)
		{
			// TODO Show tooltip
		}
	}

	private void OnDestroy()
	{
		if (!Application.isPlaying) return;

		InputController.Instance.Interact -= OnInteract;
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;
		if (!DebugManager.Instance.ShowDebugOverlay) return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, interactionRadius);
	}

	private void OnInteract()
	{
		Debug.Log("OnInteract");

		foreach (var hit in hits)
		{
			if (!hit.transform) continue;

			var rune = hit.transform.GetComponent<Rune>();
            if (rune && !carryingRune)
            {
				carryingRune = rune;
				rune.Pickup();
				rune.transform.SetParent(carryParent);
				rune.transform.localPosition = Vector3.zero;

				// TODO Move to Rune.cs
				var carryingRuneLayer = Mathf.RoundToInt(Mathf.Log(carryingRuneMask.value, 2));
				rune.gameObject.layer = carryingRuneLayer;
				return;
            }

			var slot = hit.transform.GetComponent<RuneSlot>();
			if (slot && carryingRune)
			{
				carryingRune.transform.SetParent(null);

				// TODO Move to Rune.cs
				var groundedRuneLayer = Mathf.RoundToInt(Mathf.Log(groundedRuneMask.value, 2));
				carryingRune.gameObject.layer = groundedRuneLayer;

				carryingRune.InsertIntoSlot(slot);
				carryingRune = null;
				return;
			}
        }

		if (carryingRune)
		{
			Debug.Log("Throw");

			carryingRune.transform.SetParent(null);
			carryingRune.Throw(movement.MovementDir);

			var groundedRuneLayer = Mathf.RoundToInt(Mathf.Log(groundedRuneMask.value, 2));
			carryingRune.gameObject.layer = groundedRuneLayer;
			carryingRune = null;
			return;
		}
	}
}
