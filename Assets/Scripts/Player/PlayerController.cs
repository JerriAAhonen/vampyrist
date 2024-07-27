using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
	private PlayerHealth health;
	private PlayerMovement movement;
	private PlayerInteraction interaction;

	public bool IsAlive { get; private set; }
	public bool AllowPlayerControls => IsAlive && !LevelController.Instance.GamePaused;

	protected override void Awake()
	{
		base.Awake();

		health = GetComponent<PlayerHealth>();
		movement = GetComponent<PlayerMovement>();
		interaction = GetComponent<PlayerInteraction>();
	}

	private void Start()
	{
		IsAlive = true;
		health.Init(this);
		movement.Init(this, interaction);
		interaction.Init(this, movement);
	}

	public void OnDie()
	{
		IsAlive = false;
		movement.OnDie();
		interaction.OnDie();
		LevelController.Instance.OnPlayerDied();
	}
}
