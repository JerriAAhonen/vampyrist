using UnityEngine;

public class RuneSlot : MonoBehaviour
{
	private RuneCircle circle;
	private Rune insertedRune;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		circle = GetComponentInParent<RuneCircle>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void InsertRune(Rune rune)
	{
		if (insertedRune)
			return;

        insertedRune = rune;
		rune.transform.SetParent(transform);
		rune.transform.localPosition = Vector3.zero;

		// TODO anims

		var valid = circle.SetRune(rune.Data);

		spriteRenderer.color = valid ? Color.green : Color.red;
	}

	public void RemoveRune()
	{
		if (!insertedRune)
			return;

		circle.RemoveRune(insertedRune.Data);
		insertedRune = null;

		spriteRenderer.color = Color.grey;
	}
}
