using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
	[Serializable]
	public struct SortableSprite
	{
		public SpriteRenderer spriteRenderer;
		public int relativeOrder;
	}

	[SerializeField] private Transform customOffsetTm;
	[SerializeField] private List<SortableSprite> sortableSprites;

	private int baseSortingOrder;
	private float offset;

	private void Start()
	{
		if (customOffsetTm)
			offset = customOffsetTm.localPosition.y;
	}

	private void Update()
	{
		baseSortingOrder = transform.GetSortingOrder(offset);
		foreach (var ss in sortableSprites)
		{
			ss.spriteRenderer.sortingOrder = baseSortingOrder + ss.relativeOrder;
		}
	}
}
