using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class RuneCircle : MonoBehaviour
{
	// Name : ShadowBinder's Circle

	private readonly List<RuneData> runes = new();

	private MainRuneSlot mainRuneSlot;
	private MainRuneData mainRune;
	private Portal portal;
	private float slotDistance = 2f;

	public bool Complete { get; private set; }

	private void Awake()
	{
		mainRuneSlot = GetComponentInChildren<MainRuneSlot>();
	}

	public IEnumerator Init(Portal portal, MainRuneData mainRune)
	{
		this.portal = portal;
		this.mainRune = mainRune;
		mainRuneSlot.SetRuneIcon(mainRune);
		yield return null;
	}

	public bool SetRune(RuneData rune)
	{
		runes.Add(rune);

		var isValid = mainRune.IsValidShard(rune);
		var isDuplicate = HasDuplicates(runes);

		//Debug.Log($"isValid: {isValid}, isDuplicate: {isDuplicate}");

		if (isValid && !isDuplicate && IsComplete())
			OnComplete();

		return isValid && !isDuplicate;
	}

	public void RemoveRune(RuneData rune)
	{
		runes.Remove(rune);
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
		//Debug.Log("Circle complete!");
		Complete = true;
		portal.OnCircleComplete(this);
	}

	/*private IEnumerator InitSlots()
	{
		var slotCount = mainRune.ShardAmount;

		var angleStep = 360f / slotCount;
		var startingAngle = -90f;

		if (slotCount == 2)
			startingAngle = 0f;

		for (int i = 0; i < slotCount; i++)
		{
			var slot = Instantiate(runeSlotPrefab, transform);
			var angle = startingAngle - i * angleStep;
			var position = MathUtil.CalculatePositionOnCircle(angle, slotDistance / transform.localScale.x);
			
			slot.transform.localPosition = position;
			yield return null;
		}
	}*/
}
