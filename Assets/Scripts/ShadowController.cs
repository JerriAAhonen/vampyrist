using System.Collections;
using UnityEngine;

public class ShadowController : Singleton<ShadowController>
{
	[SerializeField] private Material shadowMaterial;
	[SerializeField] private RenderTexture renderTexture;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Camera cam;

	private Texture2D texture;
	private Vector2 playerWorldPos;
	private float step;
	private float stepIncreaseSpeed;

	public bool InSunlight { get; private set; }

	public void Init(Vector2 movementSpeed, float step, float stepIncreaseSpeed)
	{
		this.step = step;
		this.stepIncreaseSpeed = stepIncreaseSpeed;

		shadowMaterial.SetVector("_Speed", movementSpeed);
		shadowMaterial.SetFloat("_Step", step);
		
		StartCoroutine(ReadPixelRoutine());
	}

	public void SetPlayerWorldPos(Vector2 pos) => playerWorldPos = pos;

	private IEnumerator ReadPixelRoutine()
	{
		while (true) // TODO
		{
			yield return new WaitForEndOfFrame();

			RenderTexture.active = renderTexture;
			cam.targetTexture = renderTexture;
			cam.enabled = true;
			cam.Render();
			cam.enabled = false;

			if (texture == null)
				texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

			texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
			texture.Apply();
			RenderTexture.active = null;

			var screenPos = cam.WorldToScreenPoint(playerWorldPos);
			int x = Mathf.RoundToInt(screenPos.x);
			int y = Mathf.RoundToInt(screenPos.y);
			var color = texture.GetPixel(x, y);

			InSunlight = color.a <= Mathf.Epsilon;

			/*InSunlight = color == Color.white;
			Debug.Log($"Color: {color} == {Color.white}: {InSunlight}");*/
			//Debug.Log(InSunlight);
		}
	}
}
