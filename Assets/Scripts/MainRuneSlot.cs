using UnityEngine;

public class MainRuneSlot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;

	public void SetRuneIcon(Sprite icon)
	{
		spriteRenderer.sprite = icon;
	}
}
