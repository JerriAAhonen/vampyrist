using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Portal : MonoBehaviour
{
	[SerializeField] private List<MainRuneSlot> slots;

	private SpriteRenderer spriteRenderer;
	private List<RuneCircle> circles;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public IEnumerator Init(List<MainRuneData> mainRuneDatas, List<RuneCircle> circles)
	{
		for (int i = 0; i < mainRuneDatas.Count; i++)
		{
			slots[i].SetRuneIcon(mainRuneDatas[i].Icon);
			yield return null;
		}

		this.circles = circles;
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
	}
}
