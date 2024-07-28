using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardsController : MonoBehaviour
{
	[SerializeField] private Rune shardPrefab;
	[SerializeField] private List<MainRuneData> allMainRuneDatas;

	private float minRadius = 8f;
	private float maxRadius = 15f;

	public IEnumerator Init(List<MainRuneData> mainRunes)
	{
		var spawnedRuneDatas = new List<RuneData>();

		foreach (var rune in mainRunes)
			foreach (var shardData in rune.Shards)
				yield return SpawnShard(shardData, rune.Color);

		var randAmountExtraShards = Random.Range(Mathf.RoundToInt(spawnedRuneDatas.Count / 2f), spawnedRuneDatas.Count);
		Debug.Log($"Spawn {randAmountExtraShards} extra shards");

		for (int i = 0; i < randAmountExtraShards; i++)
		{
			MainRuneData randMainRuneData;
			RuneData randShardData;
			var counter = 0;
			var limit = 100;
			do
			{
				randMainRuneData = allMainRuneDatas.Random();
				randShardData = randMainRuneData.Shards.Random();
				counter++;

				if (counter > limit)
				{
					Debug.Log("No more unique shards available");
					yield break;
				}

			} while (spawnedRuneDatas.Contains(randShardData));

			yield return SpawnShard(randShardData, randMainRuneData.Color);
		}

		IEnumerator SpawnShard(RuneData shardData, Color color)
		{
			spawnedRuneDatas.Add(shardData);
			Vector2 randPos;

			do
			{
				randPos = MathUtil.GetRandomPositionInCircle(minRadius, maxRadius);
			} while (Physics2D.OverlapCircle(randPos, 0.5f) != null);

			var shard = Instantiate(shardPrefab, transform);
			shard.Init(shardData, color);
			shard.transform.position = randPos;
			yield return null;
		}
	}
}
