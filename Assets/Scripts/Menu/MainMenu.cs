using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject mainPage;
	[SerializeField] private GameObject controlsPage;
	[SerializeField] private DefaultButton playButton;
	[SerializeField] private DefaultButton soundButton;
	[SerializeField] private DefaultButton controlsButton;
	[SerializeField] private DefaultButton backToMainMenuButton;
	[SerializeField] private DefaultButton quitButton;
	[SerializeField] private TextMeshProUGUI highscore;

	private void Awake()
	{
		mainPage.SetActive(true);
		controlsPage.SetActive(false);

		playButton.OnClick.AddListener(OnPlay);
		soundButton.OnClick.AddListener(OnSounds);
		controlsButton.OnClick.AddListener(OnControls);
		backToMainMenuButton.OnClick.AddListener(OnBackToMainPage);
		quitButton.OnClick.AddListener(OnQuit);

		var hs = PlayerPrefs.GetInt(LevelController.PP_Key_Highscore, -1);
		highscore.text = $"Highest Level Cleared {hs}";
		highscore.gameObject.SetActive(hs > -1);

		RefreshSoundButtonText();
	}

	private void OnPlay()
	{
		SceneLoader.Instance.LoadLevel(0);
	}

	private void OnSounds()
	{
		AudioManager.Instance.ToggleVolume();
		RefreshSoundButtonText();
	}

	private void OnControls()
	{
		mainPage.SetActive(false);
		controlsPage.SetActive(true);
	}

	private void OnBackToMainPage()
	{
		mainPage.SetActive(true);
		controlsPage.SetActive(false);
	}

	private void OnQuit()
	{
		Application.Quit();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}

	private void RefreshSoundButtonText()
	{
		if (PlayerPrefs.GetInt(AudioManager.PPKey_VolumeOn) == 1)
			soundButton.SetText("Sounds Off");
		else
			soundButton.SetText("Sounds On");
	}
}
