using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private CharacterController cc;
	private float movementSpeed = 3f;
	private Vector2 prevMovement;
	private float jumpDuration = 0.2f;
	private float jumpSpeed = 8f;
	private bool jumping;

	private void Awake()
	{
		cc = GetComponent<CharacterController>();
	}

	private void Update()
	{
		var movement = InputController.Instance.Movement;
		var jump = InputController.Instance.Jump;

		prevMovement = movement;

		if (jump && !jumping)
		{
			StartCoroutine(JumpRoutine());
		}
		else if (!jumping)
		{
			cc.Move(movementSpeed * Time.deltaTime * movement);
		}
	}

	private IEnumerator JumpRoutine()
	{
		jumping = true;
		var elapsed = 0f;

		while (elapsed < jumpDuration)
		{
			elapsed += Time.deltaTime;
			cc.Move(jumpSpeed * Time.deltaTime * prevMovement);

			yield return null;
		}

		jumping = false;
	}
}
