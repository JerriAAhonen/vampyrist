using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardsController : MonoBehaviour
{
	[SerializeField] private Rune shardPrefab;

	private float minRadius = 3f;
	private float maxRadius = 8f;

	public IEnumerator Init(List<MainRuneData> mainRunes)
	{
		foreach (var rune in mainRunes)
		{
			foreach (var shardData in rune.Shards)
			{
				var randPos = MathUtil.GetRandomPositionInCircle(minRadius, maxRadius);
				// TODO Check no overlap
				var shard = Instantiate(shardPrefab, transform);
				shard.Init(shardData);
				shard.transform.position = randPos;
				yield return null;
			}
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