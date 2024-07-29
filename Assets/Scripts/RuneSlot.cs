using UnityEngine;

public class RuneSlot : MonoBehaviour, IHighlightable
{
	[SerializeField] private RuneSlotSide side;
	[SerializeField] private GameObject highlight;
	[SerializeField] private LayerMask playerLayer;

	private RuneCircle circle;
	private Rune insertedRune;

	public bool Locked => circle.Complete;
	public bool HasRune => insertedRune != null;
	public Rune InsertedRune => insertedRune;

	public Vector3 Position => transform.position;

	private void Awake()
	{
		circle = GetComponentInParent<RuneCircle>();
		highlight.SetActive(false);
	}

	public void InsertRune(Rune rune)
	{
		if (Locked)
			return;

		insertedRune = rune;
		rune.transform.SetParent(transform);
		rune.transform.localPosition = Vector3.zero;

		// TODO anims

		var valid = circle.SetRune(rune.Data, side);
	}

	public void RemoveRune()
	{
		if (!insertedRune) return;
		if (Locked) return;

		circle.RemoveRune(insertedRune.Data, side);
		insertedRune = null;
	}

	public void ActivateHighlight(bool activate)
	{
		highlight.SetActive(activate);
	}
}
