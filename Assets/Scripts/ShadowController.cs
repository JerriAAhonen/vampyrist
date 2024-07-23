using UnityEngine;

public class ShadowController : MonoBehaviour
{
	[SerializeField] private float xOffset;
	[SerializeField] private float yOffset;
	[SerializeField] private float scale;
	[SerializeField] private float cutoff;

	private SpriteRenderer spriteRenderer;
	private Material material;
	

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		material = spriteRenderer.sharedMaterial;
	}

	private void Start()
	{
	}

	private void Update()
	{
		var pos = transform.position;
		material.SetVector("_PlayerPos", pos);
	}

	public Color GetColorAtWorldPosition(Vector2 worldPosition)
	{
		// Check if the SpriteRenderer and its sprite are available
		if (spriteRenderer == null || spriteRenderer.sprite == null)
		{
			Debug.LogError("SpriteRenderer or sprite is not assigned.");
			return Color.clear;
		}

		// Get the sprite and texture
		Sprite sprite = spriteRenderer.sprite;
		Texture2D texture = sprite.texture;

		// Get the sprite's local bounds
		Rect spriteRect = sprite.rect;
		Vector2 spriteSize = new Vector2(spriteRect.width, spriteRect.height);

		// Convert world position to local sprite space
		Vector2 localPosition = spriteRenderer.transform.InverseTransformPoint(worldPosition);

		// Convert local position to UV coordinates
		Vector2 uv = (localPosition - spriteRect.position) / spriteSize;

		// Clamp UV to ensure it's within the texture bounds
		uv = new Vector2(Mathf.Clamp01(uv.x), Mathf.Clamp01(uv.y));

		// Calculate pixel coordinates
		Vector2 pixelCoordinates = new Vector2(uv.x * texture.width, uv.y * texture.height);

		// Get the color from the texture
		Color color = texture.GetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y);

		return color;
	}
}
