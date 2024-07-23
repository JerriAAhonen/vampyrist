using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCirclesController : MonoBehaviour
{
	[SerializeField] private RuneCircle runeCirclePrefab;

	private float circleDistance = 3f;

	public List<RuneCircle> Circles { get; private set; }

	public IEnumerator SetCircles(Portal portal, List<MainRuneData> mainRuneDatas)
	{
		var runeCount = mainRuneDatas.Count;
		var angleStep = 360f / runeCount;
		var startingAngle = 90f;
		Circles = new(runeCount);

		if (runeCount == 1)
		{
			var runeCircle = Instantiate(runeCirclePrefab);
			runeCircle.transform.SetParent(transform);

			yield return runeCircle.Init(portal, mainRuneDatas[0]);
			Circles.Add(runeCircle);
			yield break;
		}

		if (runeCount == 2)
			startingAngle = 0f;

		for (int i = 0; i < runeCount; i++)
		{
			var runeCircle = Instantiate(runeCirclePrefab);
			var angle = startingAngle - i * angleStep;
			var position = MathUtil.CalculatePositionOnCircle(angle, circleDistance / transform.localScale.x);
			
			runeCircle.transform.SetParent(transform);
			runeCircle.transform.localPosition = position;

			yield return runeCircle.Init(portal, mainRuneDatas[i]);
			Circles.Add(runeCircle);
			yield return null;
		}
	}
}
