using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCirclesController : MonoBehaviour
{
	[SerializeField] private RuneCircle runeCirclePrefab;

	[SerializeField] private Transform oneRunePos;
	[SerializeField] private List<Transform> twoRunesPos;
	[SerializeField] private List<Transform> threeRunesPos;
	[SerializeField] private List<Transform> fourRunesPos;

	public List<RuneCircle> Circles { get; private set; }

	public IEnumerator SetCircles(Portal portal, List<MainRuneData> mainRuneDatas)
	{
		int count = mainRuneDatas.Count;
		Circles = new(count);

		if (count == 1)
		{
			var circle = InstantiateInsideParent(oneRunePos);

			yield return circle.Init(portal, mainRuneDatas[0]);
			Circles.Add(circle);
			yield break;
		}

		if (count == 2)
		{
			for (int i = 0; i < count; i++)
			{
				var circle = InstantiateInsideParent(twoRunesPos[i]);
				yield return circle.Init(portal, mainRuneDatas[i]);
				Circles.Add(circle);
			}
			yield break;
		}

		if (count == 3)
		{
			for (int i = 0; i < count; i++)
			{
				var circle = InstantiateInsideParent(threeRunesPos[i]);
				yield return circle.Init(portal, mainRuneDatas[i]);
				Circles.Add(circle);
			}
			yield break;
		}

		for (int i = 0; i < count; i++)
		{
			var circle = InstantiateInsideParent(fourRunesPos[i]);
			yield return circle.Init(portal, mainRuneDatas[i]);
			Circles.Add(circle);
		}

		RuneCircle InstantiateInsideParent(Transform parent)
		{
			var runeCircle = Instantiate(runeCirclePrefab);
			runeCircle.transform.SetParent(parent);
			runeCircle.transform.localPosition = Vector3.zero;
			return runeCircle;
		}
	}
}
