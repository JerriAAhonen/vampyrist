using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
	[SerializeField] private LoadingCanvas loadingCanvas;

	private int menuSceneIndex = 1;
	private int levelSceneIndex = 2;

	public void LoadMenu(bool instant = false)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return loadingCanvas.Show(instant);

			var op = SceneManager.LoadSceneAsync(menuSceneIndex, LoadSceneMode.Single);
			op.allowSceneActivation = true;
			yield return op;
			yield return loadingCanvas.Hide();
		}
	}

	public void LoadLevel(int index)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return loadingCanvas.Show(false);

			var op = SceneManager.LoadSceneAsync(levelSceneIndex, LoadSceneMode.Single);
			op.allowSceneActivation = true;
			yield return op;
			yield return null;
			Debug.Log(LevelController.Instance, LevelController.Instance);
			yield return LevelController.Instance.Init(index);
			yield return loadingCanvas.Hide();
		}
	}
}
