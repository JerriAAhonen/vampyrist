using System.Collections;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
	private bool isVisible;

	public IEnumerator Show(bool instant)
	{
		if (isVisible)
			yield break;

		isVisible = true;
		gameObject.SetActive(true);
	}

	public IEnumerator Hide()
	{
		if (!isVisible)
			yield break;

		isVisible = false;
		gameObject.SetActive(false);
	}
}
