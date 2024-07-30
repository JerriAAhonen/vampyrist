using UnityEngine;

public class MainRuneSlot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;

	public void SetRuneIcon(MainRuneData data)
	{
		gameObject.SetActive(true);
		spriteRenderer.sprite = data.Icon;
		spriteRenderer.color = data.Color;
	}

	public void RemoveIcon()
	{
		gameObject.SetActive(false);
	}
}
