using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
	[SerializeField] private RuneData data;

	private Rigidbody2D rb;
	private float throwForce = 20f;
	private float startingLinearDrag = 0f;
	private float linearDragIncrease = 0.05f;
	private bool inCarry;
	private float checkRadius = 0.4f;

	private bool inSlot;
	private RuneSlot slot;

	public RuneData Data => data;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
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

	public void Pickup()
	{
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;

		if (inSlot)
		{
			inSlot = false;
			slot.RemoveRune();
		}
	}

	public void Throw(Vector2 dir)
	{
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

	private void CheckForSlot()
	{
		var cols = Physics2D.CircleCastAll(transform.position, checkRadius, Vector2.zero);
		foreach (var col in cols)
		{
			var slot = col.collider.gameObject.GetComponent<RuneSlot>();
			if (slot)
			{
				slot.InsertRune(this);
				rb.velocity = Vector2.zero;
				rb.isKinematic = true;
				return;
			}
		}
	}
}
