using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] private List<MainRuneSlot> slots;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private Transform innerPortalParentTm;
	[SerializeField] private List<Transform> flames;
	[SerializeField] private GameObject vcam;

	private bool isComplete;
	private SpriteRenderer spriteRenderer;
	private List<RuneCircle> circles;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		innerPortalParentTm.localScale = Vector3.zero;
		foreach (var flame in flames)
			flame.localScale = Vector3.zero;

		vcam.SetActive(false);
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
			slots[i].SetRuneIcon(mainRuneDatas[i]);
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
		isComplete = true;
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			LevelController.Instance.PauseGame(true);

			vcam.SetActive(true);

			yield return new WaitForSeconds(2f);

			LeanTween.value(0f, 1f, 0.5f)
				.setOnUpdate(v => innerPortalParentTm.localScale = Vector3.one * v)
				.setEase(LeanTweenType.easeOutBack);
			LeanTween.value(0f, 1f, 0.2f)
				.setOnUpdate(v =>
				{
					foreach (var flame in flames)
						flame.localScale = (Vector3.one * 1.6f) * v;
				})
				.setEase(LeanTweenType.easeOutBack);

			yield return new WaitForSeconds(0.5f);

			vcam.SetActive(false);

			yield return new WaitForSeconds(2f);

			LevelController.Instance.PauseGame(false);
		}
	}
}
