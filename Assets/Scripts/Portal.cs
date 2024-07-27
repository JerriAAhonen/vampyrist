using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Portal : MonoBehaviour
{
	[SerializeField] private List<MainRuneSlot> slots;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private Transform innerPortalParentTm;

	private bool isComplete;
	private SpriteRenderer spriteRenderer;
	private List<RuneCircle> circles;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		innerPortalParentTm.localScale = Vector3.zero;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isComplete)
			return;

		if (BitMaskUtil.MaskContainsLayer(playerLayer, collision.gameObject.layer))
		{
			StartCoroutine(Routine());
			isComplete = false;
		}

		IEnumerator Routine()
		{
			yield return PlayerController.Instance.OnEnterPortal();
			LevelController.Instance.OnEnterPortal();
		}
	}

	public IEnumerator Init(List<MainRuneData> mainRuneDatas, List<RuneCircle> circles)
	{
		for (int i = 0; i < mainRuneDatas.Count; i++)
		{
			slots[i].SetRuneIcon(mainRuneDatas[i].Icon, mainRuneDatas[i].Color);
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

		LeanTween.value(0f, 1f, 0.5f)
			.setOnUpdate(v => innerPortalParentTm.localScale = Vector3.one * v)
			.setEase(LeanTweenType.easeOutBack);
	}
}
