using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MainRuneData", fileName = "MainRuneData")]
public class MainRuneData : ScriptableObject
{
	[SerializeField] private Sprite icon;
	[SerializeField] private List<RuneData> shards;

	public Sprite Icon => icon;
	public int ShardAmount => shards.Count;

	public bool IsValidShard(RuneData data) => shards.Contains(data);
}
