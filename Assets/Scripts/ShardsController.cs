using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardsController : MonoBehaviour
{
	[SerializeField] private Rune shardPrefab;
	[SerializeField] private List<MainRuneData> allMainRuneDatas;

	private float minRadius = 3f;
	private float maxRadius = 8f;

	public IEnumerator Init(List<MainRuneData> mainRunes)
	{
		var spawnedRuneDatas = new List<RuneData>();

		foreach (var rune in mainRunes)
			foreach (var shardData in rune.Shards)
				yield return SpawnShard(shardData);

		var randAmountExtraShards = Random.Range(Mathf.RoundToInt(spawnedRuneDatas.Count / 2f), spawnedRuneDatas.Count);
		Debug.Log($"Spawn {randAmountExtraShards} extra shards");

		for (int i = 0; i < randAmountExtraShards; i++)
		{
			RuneData randShardData;
			var counter = 0;
			var limit = 100;
			do
			{
				var randMainData = allMainRuneDatas.Random();
				randShardData = randMainData.Shards.Random();
				counter++;

				if (counter > limit)
				{
					Debug.Log("No more unique shards available");
					yield break;
				}

			} while (spawnedRuneDatas.Contains(randShardData));

			yield return SpawnShard(randShardData);
		}

		IEnumerator SpawnShard(RuneData shardData)
		{
			spawnedRuneDatas.Add(shardData);

			var randPos = MathUtil.GetRandomPositionInCircle(minRadius, maxRadius);
			// TODO Check no overlap
			var shard = Instantiate(shardPrefab, transform);
			shard.Init(shardData);
			shard.transform.position = randPos;
			yield return null;
		}
	}
}
