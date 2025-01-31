using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DefaultButton : MonoBehaviour
{
	[Serializable]
	public struct LayerRefs
	{
		public RectTransform rt;
		public Vector2 pos;
	}

	[SerializeField] private List<LayerRefs> layerRefs;
	[SerializeField] private List<TextMeshProUGUI> texts;
	[SerializeField] private Button button;
	[Space]
	[SerializeField] private AudioEvent hoverEvent;
	[SerializeField] private AudioEvent clickEvent;

	private float effectTransitionDur = 0.1f;
	private int? hoverTweenId;
	private float tweenState;

	public UnityEvent OnClick => button.onClick;

	private void Awake()
	{
		foreach (LayerRefs layerRef in layerRefs)
		{
			layerRef.rt.anchoredPosition = Vector2.zero;
		}
	}

	public void SetText(string text)
	{
		foreach (var tmpro in texts)
		{
			tmpro.text = text;
		}
	}

	public void OnHoverEnter()
	{
		AudioManager.Instance.PlayOnce(hoverEvent);

		if (hoverTweenId.HasValue)
			LeanTween.cancel(hoverTweenId.Value);

		hoverTweenId = LeanTween.value(tweenState, 1f, effectTransitionDur)
			.setOnUpdate(v =>
			{
				tweenState = v;
				foreach (var l in layerRefs)
				{
					l.rt.anchoredPosition = Vector3.Lerp(Vector3.zero, l.pos, v);
				}
			})
			.setIgnoreTimeScale(true)
			.uniqueId;
	}

	public void OnPointerClick()
	{
		AudioManager.Instance.PlayOnce(clickEvent);
	}

	public void OnHoverExit()
	{
		if (hoverTweenId.HasValue)
			LeanTween.cancel(hoverTweenId.Value);

		hoverTweenId = LeanTween.value(tweenState, 0f, effectTransitionDur)
			.setOnUpdate(v =>
			{
				tweenState = v;
				foreach (var l in layerRefs)
				{
					l.rt.anchoredPosition = Vector3.Lerp(Vector3.zero, l.pos, v);
				}
			})
			.setIgnoreTimeScale(true)
			.uniqueId;
	}
}
