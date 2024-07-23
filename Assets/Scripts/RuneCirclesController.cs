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
			runeCircle.Init(portal, mainRuneDatas[0]);
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
			runeCircle.Init(portal, mainRuneDatas[i]);
			Circles.Add(runeCircle);

			yield return null;
		}
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