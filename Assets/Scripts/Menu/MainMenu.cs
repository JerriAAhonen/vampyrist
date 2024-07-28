using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private DefaultButton playButton;
	[SerializeField] private DefaultButton soundButton;
	[SerializeField] private DefaultButton quitButton;
	[SerializeField] private TextMeshProUGUI highscore;

	private void Awake()
	{
		playButton.OnClick.AddListener(OnPlay);
		soundButton.OnClick.AddListener(OnSounds);
		quitButton.OnClick.AddListener(OnQuit);
		var hs = PlayerPrefs.GetInt(LevelController.PP_Key_Highscore, -1);
		highscore.text = $"Highscore level {hs}";
		highscore.gameObject.SetActive(hs > -1);
	}

	private void OnPlay()
	{
		SceneLoader.Instance.LoadLevel(0);
	}

	private void OnSounds()
	{

	}

	private void OnQuit()
	{
		Application.Quit();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
