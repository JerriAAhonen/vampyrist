using UnityEngine;

public class DebugManager : PersistentSingleton<DebugManager>
{
	[SerializeField] private bool showDebugOverlay;

	public bool ShowDebugOverlay => showDebugOverlay;
}