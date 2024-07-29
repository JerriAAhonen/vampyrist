using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
	[SerializeField] private Image bg;
	[SerializeField] private RectTransform batsParent;

	private bool isVisible;
	private Canvas canvas;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	public IEnumerator Show()
	{
		if (isVisible)
			yield break;

		isVisible = true;
		bg.fillAmount = 0f;
		gameObject.SetActive(true);

		batsParent.gameObject.SetActive(true);

		bg.fillOrigin = 0;
		var elapsed = 0f;
		var dur = 1f;
		var canvasWidth = canvas.pixelRect.width / canvas.scaleFactor;
		canvasWidth += 260f + 260f;

		while (elapsed < dur)
		{
			elapsed += Time.deltaTime;
			bg.fillAmount = elapsed / dur;

			var batsPos = canvasWidth * (elapsed / dur) - 260f;
			//Debug.Log($"width {canvasWidth}, elapsed / dur {elapsed / dur}");
			batsParent.anchoredPosition = new Vector3(batsPos, 0f, 0f);

			yield return null;
		}

		yield return null;
	}

	public IEnumerator Hide()
	{
		if (!isVisible)
			yield break;

		isVisible = false;

		batsParent.gameObject.SetActive(true);

		bg.fillOrigin = 1;
		var elapsed = 0f;
		var dur = 1f;
		var canvasWidth = canvas.pixelRect.width / canvas.scaleFactor;
		canvasWidth += 260f + 260f;

		while (elapsed < dur)
		{
			elapsed += Time.deltaTime;
			bg.fillAmount = 1 - elapsed / dur;

			var batsPos = canvasWidth * (elapsed / dur) - 260f;
			//Debug.Log($"width {canvasWidth}, elapsed / dur {elapsed / dur}");
			batsParent.anchoredPosition = new Vector3(batsPos, 0f, 0f);

			yield return null;
		}

		bg.fillAmount = 1f;
		gameObject.SetActive(false);
		batsParent.gameObject.SetActive(false);
		yield return null;
	}
}
