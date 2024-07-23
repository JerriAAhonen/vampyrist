using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class RuneCircle : MonoBehaviour
{
	// Name : ShadowBinder's Circle

	private readonly List<RuneData> runes = new();

	private SpriteRenderer spriteRenderer;
	private MainRuneSlot mainRuneSlot;
	private MainRuneData mainRune;
	private Portal portal;
	private float slotDistance = 2f;

	public bool Complete { get; private set; }

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		mainRuneSlot = GetComponentInChildren<MainRuneSlot>();
	}

	public void Init(Portal portal, MainRuneData mainRune)
	{
		this.portal = portal;
		this.mainRune = mainRune;
		mainRuneSlot.SetRuneIcon(mainRune.Icon);
	}

	public bool SetRune(RuneData rune)
	{
		runes.Add(rune);

		var isValid = mainRune.IsValidShard(rune);
		var isDuplicate = HasDuplicates(runes);

		Debug.Log($"isValid: {isValid}, isDuplicate: {isDuplicate}");

		if (isValid && !isDuplicate && IsComplete())
			OnComplete();

		return isValid && !isDuplicate;
	}

	public void RemoveRune(RuneData rune)
	{
		runes.Remove(rune);
		spriteRenderer.color = Color.white;
	}

	private bool HasDuplicates(List<RuneData> list)
	{
		HashSet<RuneData> set = new();

		foreach (var item in list)
		{
			if (!set.Add(item))
			{
				// If Add returns false, the item is already in the set, indicating a duplicate
				return true;
			}
		}

		return false;
	}

	private bool IsComplete()
	{
		foreach (var rune in runes)
		{
			if (!mainRune.IsValidShard(rune))
				return false;
		}

		if (HasDuplicates(runes))
			return false;

		if (runes.Count < mainRune.ShardAmount)
			return false;

		return true;
	}

	private void OnComplete()
	{
		// TODO Activate circle and inform portal
		Debug.Log("Circle complete!");
		spriteRenderer.color = Color.green;
		Complete = true;
		portal.OnCircleComplete(this);
	}

	[Button("Set Slots")]
	private void SpreadSlotsAlongCircle()
	{
		var childCount = transform.childCount;
		if (childCount == 0)
		{
			Debug.LogWarning("No child objects found to distribute.");
			return;
		}

		var angleStep = 360f / childCount;
		var startingAngle = 90f;

		if (childCount == 2)
			startingAngle = 0f;

		for (int i = 0; i < childCount; i++)
		{
			var child = transform.GetChild(i);
			var angle = startingAngle - i * angleStep;
			var position = MathUtil.CalculatePositionOnCircle(angle, slotDistance / transform.localScale.x);
			child.localPosition = position;
		}
	}
}
