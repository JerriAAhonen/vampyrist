using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 3f;
	[SerializeField] private float jumpDuration = 0.2f;
	[SerializeField] private float jumpSpeed = 8f;
	[SerializeField] private Transform visualTm;
	[SerializeField] private Transform animParentTm;

	private PlayerController controller;
	private PlayerInteraction interaction;
	private Rigidbody2D rb;
	private Vector2 prevMovement;
	private bool jumping;
	private Vector3 targetVisualScale = Vector3.one;
	private Vector3 targetAnimScale = Vector3.one;
	private float startMovingTime = -1f;

	public Vector2 MovementDir { get; private set; }

	public void Init(PlayerController controller, PlayerInteraction interaction)
	{
		this.controller = controller;
		this.interaction = interaction;
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		InputController.Instance.Jump += OnJump;
	}

	private void FixedUpdate()
	{
		if (!controller.AllowPlayerControls)
		{
			rb.velocity = Vector2.zero;
			return;
		}

		var movement = InputController.Instance.Movement;
		if (movement.sqrMagnitude > 0f)
		{
			prevMovement = movement;
			MovementDir = movement;
		}

		if (!jumping)
		{
			rb.velocity = movementSpeed * Time.fixedDeltaTime * movement;
		}
		
		if (rb.velocity.sqrMagnitude > 0f)
		{
			targetVisualScale = movement.x >= 0 ? new Vector3(1, visualTm.localScale.y, visualTm.localScale.z) : new Vector3(-1, visualTm.localScale.y, visualTm.localScale.z);
			
			if (startMovingTime < 0f)
				startMovingTime = 0f;
			var bounceOffset = 1f + (Mathf.Abs(Mathf.Sin((Time.time - startMovingTime) * 10f)) * 0.2f);
			animParentTm.localScale = new Vector3(1f, bounceOffset, 1f);
		}
        else
        {
			animParentTm.localScale = Vector3.MoveTowards(animParentTm.localScale, Vector3.one, Time.fixedDeltaTime * 10f);
        }

        visualTm.localScale = Vector3.Lerp(visualTm.localScale, targetVisualScale, Time.fixedDeltaTime * 20f);
	}

	private void OnDestroy()
	{
		if (InputController.Instance)
			InputController.Instance.Jump -= OnJump;
	}

	public void OnDie()
	{
		if (InputController.Instance)
			InputController.Instance.Jump -= OnJump;
	}

	private void OnJump()
	{
		if (!controller.AllowPlayerControls)
			return;

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
