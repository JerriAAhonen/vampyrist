using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneCircle : MonoBehaviour
{
	// Name : ShadowBinder's Circle

	private readonly List<RuneData> runes = new();

	public void SetRune(RuneData rune)
	{
		runes.Add(rune);

		// TODO Check for completion
	}

	public void RemoveRune(RuneData rune)
	{
		runes.Remove(rune);
	}
}
