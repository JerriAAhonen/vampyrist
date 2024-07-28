using UnityEngine;

public class MainRuneSlot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;

	public void SetRuneIcon(MainRuneData data)
	{
		spriteRenderer.sprite = data.Icon;
		spriteRenderer.color = data.Color;
	}
}
