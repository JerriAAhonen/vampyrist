﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField] private List<LevelSettings> levelSettings;

	private List<MainRuneData> mainRunes;
	private int levelIndex;
	private LevelSettings currentSettings;

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
		GetLevelSettings();
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
		var runeCount = UnityEngine.Random.Range(1, 4); // 1 2 3
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
		Debug.Log(runeCirclesController, runeCirclesController);
		yield return runeCirclesController.SetCircles(portal, mainRunes);
	}

	private IEnumerator SetupPortal()
	{
		Debug.Log(portal, portal);
		yield return portal.Init(mainRunes, runeCirclesController.Circles);
	}

	private IEnumerator SetupRuneShards()
	{
		Debug.Log(shardsController, shardsController);
		yield return shardsController.Init(mainRunes);
	}

	private IEnumerator SetupShadowSystem()
	{
		Debug.Log(ShadowController.Instance, ShadowController.Instance);
		yield return null;
		var movementSpeed = currentSettings.GetShadowMovementSpeed();
		var step = currentSettings.GetStartingStep();
		var stepIncreaseSpeed = currentSettings.GetStepIncreaseSpeed();
		ShadowController.Instance.Init(movementSpeed, step, stepIncreaseSpeed);
	}

	private void GetLevelSettings()
	{
		foreach (var settings in levelSettings)
		{
			if (settings.IsInsideLevelRange(levelIndex))
			{
				Debug.Log($"[LevelController] using {settings} with levelIndex {levelIndex}");
				currentSettings = settings;
				return;
			}
		}

		currentSettings = levelSettings[^1];
		Debug.LogError($"[LevelController] No settings found for level {levelIndex}, using: {currentSettings}");
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
		pauseRoot.SetActive(true);
		GamePauseStateChanged?.Invoke(true);
		Time.timeScale = 0f;
	}

	private void OnContinue()
	{
		GamePaused = false;
		pauseRoot.SetActive(false);
		GamePauseStateChanged?.Invoke(false);
		Time.timeScale = 1f;
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
		SceneLoader.Instance.LoadLevel(0);
	}

	private void OnMainMenu()
	{
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