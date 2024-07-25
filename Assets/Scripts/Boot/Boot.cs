using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class Boot : MonoBehaviour
{
	// Index of the boot scene in the build settings
	private static readonly int sceneIndex = 0;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void Init()
	{
		Debug.Log("Boot.Init()");
#if UNITY_EDITOR
		// Set the boot scene to be the play mode start scene when running in the editor
		// This will cause the bootstrapper scene to be loaded first (and only once) when entering
		// play mode from the Unity Editor, regardless of which scene is currently active.
		EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[sceneIndex].path);
#endif
	}

	private void Start()
	{
		Debug.Log("Boot.Start()");
		SceneLoader.Instance.LoadMenu(true);
	}
}
