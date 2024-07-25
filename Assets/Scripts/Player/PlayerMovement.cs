using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 3f;
	[SerializeField] private float jumpDuration = 0.2f;
	[SerializeField] private float jumpSpeed = 8f;

	private PlayerInteraction interaction;
	private Rigidbody2D rb;
	private Vector2 prevMovement;
	private bool jumping;

	public Vector2 MovementDir { get; private set; }

	private void Awake()
	{
		interaction = GetComponent<PlayerInteraction>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		InputController.Instance.Jump += OnJump;
	}

	private void FixedUpdate()
	{
		var movement = InputController.Instance.Movement;
		if (movement.sqrMagnitude > 0f)
		{
			prevMovement = movement;
			MovementDir = movement;
		}

		if (!jumping)
		{
			rb.velocity = movementSpeed * Time.deltaTime * movement;
		}
	}

	private void OnDestroy()
	{
		if (InputController.Instance)
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
			rb.velocity = jumpSpeed * Time.deltaTime * prevMovement;

			yield return new WaitForFixedUpdate();
		}

		jumping = false;
	}
}
