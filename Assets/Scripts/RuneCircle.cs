using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneCircle : MonoBehaviour
{
	// Name : ShadowBinder's Circle

	private readonly List<RuneData> runes = new();

	private MainRuneData mainRune;

	public void SetMainRune(MainRuneData main)
	{
		mainRune = main;
	}

	public bool SetRune(RuneData rune)
	{
		runes.Add(rune);

		// TODO Check for completion
		var isValid = mainRune.IsValidShard(rune);
		Debug.Log($"isValid: {isValid}");

		return isValid;
	}

	public void RemoveRune(RuneData rune)
	{
		runes.Remove(rune);
	}
}
