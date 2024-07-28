using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
	public Vector2 Movement { get; private set; }
	public event Action Jump;
	public event Action Interact;
	public event Action Pause;

	private void Update()
	{
		Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
		
		if (Input.GetKeyDown(KeyCode.Space)) Jump?.Invoke();
		if (Input.GetMouseButtonDown(0)) Interact?.Invoke();
		if (Input.GetKeyDown(KeyCode.Escape)) Pause?.Invoke();
	}
}
