using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortingOrder : MonoBehaviour
{
	[SerializeField] private Transform customOffsetTm;

	private void Start()
	{
		var offset = 0f;
		if (customOffsetTm)
			offset = customOffsetTm.localPosition.y;

		var sr = GetComponent<SpriteRenderer>();
		sr.sortingOrder = transform.GetSortingOrder(offset);
	}
}
