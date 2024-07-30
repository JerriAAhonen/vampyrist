using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSettings : ScriptableObject
{
	[SerializeField] private int maxLevelIndex;
	[SerializeField] private AnimationCurve shadowSpeed;
	[SerializeField] private AnimationCurve shadowStep;
	[SerializeField] private AnimationCurve shadowStepIncrease;
	[SerializeField] private AnimationCurve runeCount;

	public int GetRuneCount(int levelIndex) => Mathf.RoundToInt(GetEvaluation(levelIndex, runeCount));
	public Vector2 GetShadowMovementSpeed(int levelIndex)
	{
		var dir = Random.insideUnitCircle;
		var speed = GetEvaluation(levelIndex, shadowSpeed);
		dir *= speed;

		return dir;
	}
	public float GetStartingStep(int levelIndex) => GetEvaluation(levelIndex, shadowStep);
	public float GetStepIncreaseSpeed(int levelIndex) => GetEvaluation(levelIndex, shadowStepIncrease);

	private float GetEvaluation(int levelIndex, AnimationCurve curve) => curve.Evaluate(GetEvaluationTime(levelIndex));
	private float GetEvaluationTime(int levelIndex) => Mathf.Clamp01((float)levelIndex / maxLevelIndex);
}
