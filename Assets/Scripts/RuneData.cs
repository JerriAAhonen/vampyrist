using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName ="RuneData", fileName ="RuneData")]
public class RuneData : ScriptableObject
{
	[ShowAssetPreview]
	[SerializeField] private Sprite icon;

	public Sprite Icon => icon;
}

/*#if UNITY_EDITOR
	[UnityEditor.MenuItem("Assets/Generate RuneDatas")]
	public static void EDITOR_GenerateRuneDatas()
	{
		var path = "";
		var obj = UnityEditor.Selection.activeObject;

		if (obj == null) path = "Assets";
		else path = UnityEditor.AssetDatabase.GetAssetPath(obj.GetInstanceID());

		if (path.Length > 0)
		{
			if (Directory.Exists(path))
			{
				Debug.Log("Folder");
			}
			else
			{
				Debug.LogError("File");
				return;
			}
		}
		else
		{
			Debug.LogError("Not in assets folder");
			return;
		}

		if (Directory.Exists(path)) 
			Directory.Delete(path, true);
		Directory.CreateDirectory(path);

		foreach (RuneType type in Enum.GetValues(typeof(RuneType)))
		{
			var ins = ScriptableObject.CreateInstance<RuneData>();
			ins.type = type;

			UnityEditor.AssetDatabase.CreateAsset(ins, Path.Combine(path, $"{type}.asset"));
		}

		UnityEditor.AssetDatabase.SaveAssets();
		UnityEditor.AssetDatabase.Refresh();
	}
#endif*/

