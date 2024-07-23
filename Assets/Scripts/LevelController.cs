using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[Header("Portal")]
	[SerializeField] private List<MainRuneData> mainRuneDatas;
	[SerializeField] private Portal portal;
	[Header("Rune Circles")]
	[SerializeField] private RuneCirclesController runeCirclesController;

	private List<MainRuneData> mainRunes;

	private IEnumerator Start()
	{
		yield return RandomizeLevel();
		yield return SetupRuneCircles();
		yield return SetupPortal();
		yield return SetupRuneShards();
		yield return SetupShadowSystem();
	}

	private IEnumerator RandomizeLevel()
	{
		var runeCount = Random.Range(1, 4); // 1 2 3
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
		yield return null;
	}

	private IEnumerator SetupShadowSystem()
	{
		yield return null;
	}
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