using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
	public Vector2 Movement { get; private set; }
	public bool Jump { get; private set; }

	private void Update()
	{
		Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
		Jump = Input.GetKeyDown(KeyCode.Space);
	}
}
