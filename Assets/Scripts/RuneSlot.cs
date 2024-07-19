using UnityEngine;

public class RuneSlot : MonoBehaviour
{
	private RuneCircle circle;
	private Rune insertedRune;

	private void Awake()
	{
		circle = GetComponentInParent<RuneCircle>();
	}

	public void InsertRune(Rune rune)
	{
		if (insertedRune)
			return;

        insertedRune = rune;
		rune.transform.SetParent(transform);
		rune.transform.localPosition = Vector3.zero;

		// TODO anims

		circle.SetRune(rune.Data);
	}

	public void RemoveRune()
	{
		if (!insertedRune)
			return;

		circle.RemoveRune(insertedRune.Data);
		insertedRune = null;
	}
}
