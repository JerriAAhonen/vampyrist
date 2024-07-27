using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSettings : ScriptableObject
{
	[SerializeField] private Vector2Int levelRange;
	[Header("Runes")]
	[Tooltip("Inclusive")]
	[SerializeField] private Vector2Int mainRuneCountRange;
	[Header("Shadow")]
	[SerializeField] private Vector2 movementSpeedMin;
	[SerializeField] private Vector2 movementSpeedMax;
	[SerializeField] private Vector2 startingStepRange;
	[SerializeField] private Vector2 stepRangeIncreaseSpeedRange;

	public bool IsInsideLevelRange(int level) => levelRange.IsWithinXY(level);
	public int GetRuneCount() => mainRuneCountRange.Random();
	public Vector2 GetShadowMovementSpeed()
	{
		int multiplier = 1;
		if (Random.Range(0, 2) > 0)
			multiplier = -1;

		var x = Random.Range(movementSpeedMin.x, movementSpeedMax.x);
		var y = Random.Range(movementSpeedMin.y, movementSpeedMax.y);

		//Debug.Log($"xMin: {movementSpeedMin.x}, xMax: {movementSpeedMax.x}, = x = {x}");
		//Debug.Log($"yMin: {movementSpeedMin.y}, yMax: {movementSpeedMax.y}, = y = {y}");

		x *= multiplier;
		y *= multiplier;

		return new Vector2(x, y);
	}
	public float GetStartingStep() => startingStepRange.Random();
	public float GetStepIncreaseSpeed() => stepRangeIncreaseSpeedRange.Random();
}
