using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Rune : MonoBehaviour, IHighlightable
{
	[SerializeField] private RuneData data;
	[SerializeField] private SpriteRenderer icon;
	[SerializeField] private GameObject baseOutline;
	[SerializeField] private Light2D glow;
	[SerializeField] private LayerMask groundedRuneMask;
	[SerializeField] private LayerMask carryingRuneMask;

	private RuneSlot slot;
	private Rigidbody2D rb;
	private float throwForce = 20f;
	private float startingLinearDrag = 0f;
	private float linearDragIncrease = 0.05f;
	private bool inCarry;
	private float checkRadius = 0.4f;

	public RuneData Data => data;
	public bool CanPickup => slot == null || !slot.Locked;
	public Vector3 Position => transform.position;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		baseOutline.SetActive(false);
	}

	private void FixedUpdate()
	{
		if (!inCarry && rb.velocity.sqrMagnitude > 0f)
		{
			CheckForSlot();
		}
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;
		if (!DebugManager.Instance.ShowDebugOverlay) return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, checkRadius);
	}

	public void Init(RuneData data, MainRuneData mainRuneData)
	{
		this.data = data;
		icon.sprite = data.Icon;
		//icon.color = mainRuneData.Color;
		glow.color = mainRuneData.Color;
	}

	public void Pickup(Transform parent)
	{
		if (slot && slot.Locked)
			return;

		rb.velocity = Vector3.zero;
		rb.isKinematic = true;

		if (slot)
		{
			slot.RemoveRune();
			slot = null;
		}

		transform.SetParent(parent);
		transform.localPosition = Vector3.zero;

		SetCarrying();
	}

	public void Throw(Vector2 dir)
	{
		SetGrounded();
		transform.SetParent(null);

		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			rb.isKinematic = false;
			rb.drag = startingLinearDrag;

			yield return null;

			Debug.Log($"Throw {dir * throwForce}");
			rb.AddForce(dir * throwForce, ForceMode2D.Impulse);

			while (rb.velocity.sqrMagnitude > 0f)
			{
				rb.drag += linearDragIncrease;
				yield return null;
			}
		}
	}

	public void InsertIntoSlot(RuneSlot slot)
	{
		if (slot.HasRune)
			return;

		this.slot = slot;

		slot.InsertRune(this);
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;

		SetGrounded();
	}

	public void ActivateHighlight(bool activate)
	{
		baseOutline.SetActive(activate);
	}

	private void CheckForSlot()
	{
		var cols = Physics2D.CircleCastAll(transform.position, checkRadius, Vector2.zero);
		foreach (var col in cols)
		{
			var slot = col.collider.gameObject.GetComponent<RuneSlot>();
			if (slot)
			{
				InsertIntoSlot(slot);
				return;
			}
		}
	}

	private void SetGrounded()
	{
		var groundedRuneLayer = Mathf.RoundToInt(Mathf.Log(groundedRuneMask.value, 2));
		gameObject.layer = groundedRuneLayer;
	}

	private void SetCarrying()
	{
		var carryingRuneLayer = Mathf.RoundToInt(Mathf.Log(carryingRuneMask.value, 2));
		gameObject.layer = carryingRuneLayer;
	}
}
