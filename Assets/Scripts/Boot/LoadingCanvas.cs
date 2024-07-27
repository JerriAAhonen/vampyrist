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

		if (instant)
			yield break;

		yield return null;
		yield return null;
		yield return null;
		yield return null;
	}

	public IEnumerator Hide()
	{
		if (!isVisible)
			yield break;

		isVisible = false;
		gameObject.SetActive(false);

		yield return null;
		yield return null;
		yield return null;
		yield return null;
	}
}
