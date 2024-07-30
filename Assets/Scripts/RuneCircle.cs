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
	[SerializeField] private Image bottomWhite;
	[SerializeField] private Image right;
	[SerializeField] private Image rightWhite;
	[SerializeField] private Image left;
	[SerializeField] private Image leftWhite;
	[SerializeField] private Image center;
	[SerializeField] private Image centerWhite;
	[Space]
	[SerializeField] private AudioEvent validShardEvent;

	private readonly List<RuneData> runes = new();

	private MainRuneSlot mainRuneSlot;
	private MainRuneData mainRune;
	private Portal portal;

	public bool Complete { get; private set; }

	private void Awake()
	{
		mainRuneSlot = GetComponentInChildren<MainRuneSlot>();
		bottom.fillAmount = 0f;
		bottomWhite.fillAmount = 1f;
		right.fillAmount = 0f;
		rightWhite.fillAmount = 1f;
		left.fillAmount = 0f;
		leftWhite.fillAmount = 1f;
		center.fillAmount = 0f;
		centerWhite.fillAmount = 1f;
	}

	public IEnumerator Init(Portal portal, MainRuneData mainRune)
	{
		this.portal = portal;
		this.mainRune = mainRune;
		mainRuneSlot.SetRuneIcon(mainRune);
		
		bottom.color = mainRune.Color;
		right.color = mainRune.Color;
		left.color = mainRune.Color;
		center.color = mainRune.Color;
		yield return null;
	}

	public bool SetRune(RuneData rune, RuneSlotSide side)
	{
		runes.Add(rune);

		var isValid = mainRune.IsValidShard(rune);
		var isDuplicate = HasDuplicates(runes);

		//Debug.Log($"isValid: {isValid}, isDuplicate: {isDuplicate}");

		if (isValid)
		{
			AnimateSide(side, true);
			AudioManager.Instance.PlayOnce(validShardEvent);
		}

		if (isValid && !isDuplicate && IsComplete())
			OnComplete();

		return isValid && !isDuplicate;
	}

	public void RemoveRune(RuneData rune, RuneSlotSide side)
	{
		runes.Remove(rune);

		var wasValid = mainRune.IsValidShard(rune);
		if (wasValid)
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
				.setOnUpdate(v =>
				{
					bottom.fillAmount = v;
					bottomWhite.fillAmount = 1 - v;
				});
			return;
		}

		if (side == RuneSlotSide.Right)
		{
			LeanTween.value(from, to, 0.2f)
				.setOnUpdate(v =>
				{
					right.fillAmount = v;
					rightWhite.fillAmount = 1 - v;
				});
			return;
		}

		if (side == RuneSlotSide.Left)
		{
			LeanTween.value(from, to, 0.2f)
				.setOnUpdate(v =>
				{
					left.fillAmount = v;
					leftWhite.fillAmount = 1 - v;
				});
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

		LeanTween.value(0f, 1f, 1f)
			.setOnUpdate(v =>
			{
				center.fillAmount = v;
				centerWhite.fillAmount = 1 - v;
			});
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
