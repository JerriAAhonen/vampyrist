using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "MainRuneData", fileName = "MainRuneData")]
public class MainRuneData : ScriptableObject
{
	[ShowAssetPreview]
	[SerializeField] private Sprite icon;
	[SerializeField] private Color color;
	[SerializeField] private List<RuneData> shards;

	public Sprite Icon => icon;
	public Color Color => color;
	public int ShardAmount => shards.Count;
	public IReadOnlyList<RuneData> Shards => shards;

	public bool IsValidShard(RuneData data) => shards.Contains(data);
}
