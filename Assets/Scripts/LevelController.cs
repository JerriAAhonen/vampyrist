using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
	public const string PP_Key_Highscore = "Highscore";

	[Header("Portal")]
	[SerializeField] private List<MainRuneData> mainRuneDatas;
	[SerializeField] private Portal portal;
	[Header("Rune Circles")]
	[SerializeField] private RuneCirclesController runeCirclesController;
	[Header("Rune Shards")]
	[SerializeField] private ShardsController shardsController;
	[Header("UI")]
	[SerializeField] private GameObject failRoot;
	[SerializeField] private DefaultButton failRestartButton;
	[SerializeField] private DefaultButton failMainMenuButton;
	[Space]
	[SerializeField] private GameObject pauseRoot;
	[SerializeField] private DefaultButton pauseContinueButton;
	[SerializeField] private DefaultButton pauseMainMenuButton;
	[Space]
	[SerializeField] private TextMeshProUGUI currentLevelText;
	[Header("Level Settings")]
	[SerializeField] private LevelSettings levelSettings;

	private List<MainRuneData> mainRunes;
	private int levelIndex;

	public bool GamePaused { get; private set; }
	public event Action<bool> GamePauseStateChanged;

	protected override void Awake()
	{
		base.Awake();

		failRoot.SetActive(false);
		failRestartButton.OnClick.AddListener(OnRestart);
		failMainMenuButton.OnClick.AddListener(OnMainMenu);

		pauseRoot.SetActive(false);
		pauseContinueButton.OnClick.AddListener(OnContinue);
		pauseMainMenuButton.OnClick.AddListener(OnMainMenu);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
			OnEnterPortal();
	}

	#region Init

	public IEnumerator Init(int levelIndex)
	{
		this.levelIndex = levelIndex;
		currentLevelText.text = $"Level {levelIndex + 1}";

		yield return RandomizeLevel();
		yield return SetupRuneCircles();
		yield return SetupPortal();
		yield return SetupRuneShards();
		yield return SetupShadowSystem();

		InputController.Instance.Pause += OnPause;
	}

	protected override void OnDestroy()
	{
		if (InputController.Instance)
			InputController.Instance.Pause -= OnPause;
		base.OnDestroy();
	}

	private IEnumerator RandomizeLevel()
	{
		var runeCount = levelSettings.GetRuneCount(levelIndex);
		runeCount = 1; // TEMP

		mainRunes = new List<MainRuneData>(runeCount);

		for (int i = 0; i < runeCount; i++)
		{
			MainRuneData randRune;
			do randRune = mainRuneDatas.Random();
			while (mainRunes.Contains(randRune));

			mainRunes.Add(randRune);

			Debug.Log($"[LevelController] Chosen rune: {randRune}");

			yield return null;
		}
	}

	private IEnumerator SetupRuneCircles()
	{
		yield return runeCirclesController.SetCircles(portal, mainRunes);
	}

	private IEnumerator SetupPortal()
	{
		yield return portal.Init(mainRunes, runeCirclesController.Circles);
	}

	private IEnumerator SetupRuneShards()
	{
		yield return shardsController.Init(mainRunes);
	}

	private IEnumerator SetupShadowSystem()
	{
		yield return null;
		var movementSpeed = levelSettings.GetShadowMovementSpeed(levelIndex);
		var step = levelSettings.GetStartingStep(levelIndex);
		var stepIncreaseSpeed = levelSettings.GetStepIncreaseSpeed(levelIndex);
		ShadowController.Instance.Init(movementSpeed, step, stepIncreaseSpeed);
	}

	#endregion

	#region Pause

	public void PauseGame(bool pause)
	{
		GamePaused = pause;
		GamePauseStateChanged?.Invoke(pause);
	}

	private void OnPause()
	{
		GamePaused = true;
		Time.timeScale = 0f;
		pauseRoot.SetActive(true);
		GamePauseStateChanged?.Invoke(true);
	}

	private void OnContinue()
	{
		GamePaused = false;
		Time.timeScale = 1f;
		pauseRoot.SetActive(false);
		GamePauseStateChanged?.Invoke(false);
	}

	#endregion

	#region Success

	public void OnEnterPortal()
	{
		SceneLoader.Instance.LoadLevel(levelIndex + 1);
	}

	#endregion

	#region Fail

	public void OnPlayerDied()
	{
		failRoot.SetActive(true);

		var hs = PlayerPrefs.GetInt(PP_Key_Highscore, -1);
		if (levelIndex > hs)
			PlayerPrefs.SetInt(PP_Key_Highscore, levelIndex);
	}

	private void OnRestart()
	{
		Time.timeScale = 1f;
		SceneLoader.Instance.LoadLevel(0);
	}

	private void OnMainMenu()
	{
		Time.timeScale = 1f;
		SceneLoader.Instance.LoadMenu();
	}

	#endregion
}

/*
Setup portal:
- Rand rune count 1-3
- Rand runes
- Show runes on portal edge

Setup circle(s)
- Set main rune
- Show main run in middle
- Get shard count, set rune slots

Setup rune shards
- Get required shards
- Rand extra shards
- distribute shards into env

Setup shadow system
- params
	- density
	- movement dir
	- movement spd
	- movement spd increase
	- density decrease spd
	
- Rand params 
 */