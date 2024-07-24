using System.Collections;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
	[SerializeField] private float xOffset;
	[SerializeField] private float yOffset;
	[SerializeField] private float scale;
	[SerializeField] private float step;
	[SerializeField] private Material shadowMaterial;
	[SerializeField] private RenderTexture renderTexture;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Camera cam;

	private Texture2D texture;

	private void Awake()
	{
	}

	private void Start()
	{

	}

	public bool GetIsInSunlight(Vector2 worldPos)
	{
		RenderTexture.active = renderTexture;
		cam.targetTexture = renderTexture;
		cam.enabled = true;
		cam.Render();
		cam.enabled = false;

		if (texture == null)
			texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

		texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		texture.Apply();
		RenderTexture.active = null;

		var screenPos = cam.WorldToScreenPoint(worldPos);
		int x = Mathf.RoundToInt(screenPos.x);
		int y = Mathf.RoundToInt(screenPos.y);
		var color = texture.GetPixel(x, y);
		var inSunlight = color == Color.white;

		//Debug.Log($"pos: ({x}, {y}), Color: {color}, inSunlight: {inSunlight}");
		return inSunlight;
	}
}
