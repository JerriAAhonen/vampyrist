using UnityEngine;

public class MainRuneSlot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;

	public void SetRuneIcon(Sprite icon, Color color)
	{
		spriteRenderer.sprite = icon;
		spriteRenderer.color = color;
	}
}
