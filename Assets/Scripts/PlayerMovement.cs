using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private PlayerInteraction interaction;
	private CharacterController cc;
	private float movementSpeed = 3f;
	private Vector2 prevMovement;
	private float jumpDuration = 0.2f;
	private float jumpSpeed = 8f;
	private bool jumping;

	public Vector2 MovementDir { get; private set; }

	private void Awake()
	{
		interaction = GetComponent<PlayerInteraction>();
		cc = GetComponent<CharacterController>();
	}

	private void Start()
	{
		InputController.Instance.Jump += OnJump;
	}

	private void Update()
	{
		var movement = InputController.Instance.Movement;
		if (movement.sqrMagnitude > 0f)
		{
			prevMovement = movement;
			MovementDir = movement;
		}

		if (!jumping)
		{
			cc.Move(movementSpeed * Time.deltaTime * movement);
		}
	}

	private void OnDestroy()
	{
		if (!Application.isPlaying) return;

		InputController.Instance.Jump -= OnJump;
	}

	private void OnJump()
	{
		if (!jumping && !interaction.IsCarrying)
			StartCoroutine(JumpRoutine());
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
