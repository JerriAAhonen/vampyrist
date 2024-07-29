using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public enum RuneSlotSide
{
	Bottom, Right, Left
}

public class RuneCircle : MonoBehaviour
{
	// Name : ShadowBinder's Circle

	[SerializeField] private Image bottom;
	[SerializeField] private Image right;
	[SerializeField] private Image left;
	[SerializeField] private Image center;

	private readonly List<RuneData> runes = new();

	private MainRuneSlot mainRuneSlot;
	private MainRuneData mainRune;
	private Portal portal;

	public bool Complete { get; private set; }

	private void Awake()
	{
		mainRuneSlot = GetComponentInChildren<MainRuneSlot>();
		bottom.fillAmount = 0f;
		right.fillAmount = 0f;
		left.fillAmount = 0f;
		center.fillAmount = 0f;
	}

	public IEnumerator Init(Portal portal, MainRuneData mainRune)
	{
		this.portal = portal;
		this.mainRune = mainRune;
		mainRuneSlot.SetRuneIcon(mainRune);
		yield return null;
	}

	public bool SetRune(RuneData rune, RuneSlotSide side)
	{
		runes.Add(rune);

		var isValid = mainRune.IsValidShard(rune);
		var isDuplicate = HasDuplicates(runes);

		//Debug.Log($"isValid: {isValid}, isDuplicate: {isDuplicate}");

		AnimateSide(side, true);

		if (isValid && !isDuplicate && IsComplete())
			OnComplete();

		return isValid && !isDuplicate;
	}

	public void RemoveRune(RuneData rune, RuneSlotSide side)
	{
		runes.Remove(rune);
		AnimateSide(side, false);
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

	private void AnimateSide(RuneSlotSide side, bool hasRune)
	{
		var from = hasRune ? 0f : 1f;
		var to = hasRune ? 1f : 0f;

		if (side == RuneSlotSide.Bottom)
		{
			LeanTween.value(from, to, 0.2f)
				.setOnUpdate(v => bottom.fillAmount = v);
			return;
		}

		if (side == RuneSlotSide.Right)
		{
			LeanTween.value(from, to, 0.2f)
				.setOnUpdate(v => right.fillAmount = v);
			return;
		}

		if (side == RuneSlotSide.Left)
		{
			LeanTween.value(from, to, 0.2f)
				.setOnUpdate(v => left.fillAmount = v);
			return;
		}
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
		Complete = true;
		portal.OnCircleComplete(this);

		IEnumerator Routine()
		{
			yield return null;
		}
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
