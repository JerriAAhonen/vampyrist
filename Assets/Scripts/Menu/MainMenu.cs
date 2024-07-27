using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button playButton;
	[SerializeField] private TextMeshProUGUI highscore;

	private void Awake()
	{
		playButton.onClick.AddListener(OnPlay);
		var hs = PlayerPrefs.GetInt(LevelController.PP_Key_Highscore, -1);
		highscore.text = $"Highscore level {hs}";
		highscore.gameObject.SetActive(hs > -1);
	}

	private void OnPlay()
	{
		SceneLoader.Instance.LoadLevel(0);
	}
}
