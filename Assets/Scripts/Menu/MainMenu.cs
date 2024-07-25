using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button playButton;

	private void Awake()
	{
		playButton.onClick.AddListener(OnPlay);
	}

	private void OnPlay()
	{
		SceneLoader.Instance.LoadLevel(0);
	}
}
