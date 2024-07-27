using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Portal : MonoBehaviour
{
	[SerializeField] private List<MainRuneSlot> slots;
	[SerializeField] private LayerMask playerLayer;

	private bool isComplete;
	private SpriteRenderer spriteRenderer;
	private List<RuneCircle> circles;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isComplete)
			return;

		if (BitMaskUtil.MaskContainsLayer(playerLayer, collision.gameObject.layer))
		{
			LevelController.Instance.OnEnterPortal();
			isComplete = false;
		}
	}

	public IEnumerator Init(List<MainRuneData> mainRuneDatas, List<RuneCircle> circles)
	{
		for (int i = 0; i < mainRuneDatas.Count; i++)
		{
			slots[i].SetRuneIcon(mainRuneDatas[i].Icon);
			yield return null;
		}

		this.circles = circles;
		isComplete = false;
	}

	public void OnCircleComplete(RuneCircle circle)
	{
		if (IsComplete())
			OnComplete();
	}

	private bool IsComplete()
	{
		foreach (RuneCircle circle in circles)
			if (!circle.Complete)
				return false;
		return true;
	}

	private void OnComplete()
	{
		// TODO Activate portal
		spriteRenderer.color = Color.green;
		isComplete = true;
	}
}
