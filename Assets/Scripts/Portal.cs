using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private List<RuneCircle> circles;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void OnCircleComplete(RuneCircle circle)
	{

	}
}
