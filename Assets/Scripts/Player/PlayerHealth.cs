using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float sunlightDamageSpeed;
	[SerializeField] private float maxTimeInSunlight;
	[SerializeField] private Image fill;

	private PlayerController controller;
	private float timeInSunlight;

	public void Init(PlayerController controller)
	{
		this.controller = controller;
	}

	private void Update()
	{
		if (!controller.AllowPlayerControls)
			return;

		ShadowController.Instance.SetPlayerWorldPos(transform.position);
		var inSunlight = ShadowController.Instance.InSunlight;
		if (inSunlight)
			timeInSunlight += sunlightDamageSpeed * Time.deltaTime;
		else
			timeInSunlight -= sunlightDamageSpeed * Time.deltaTime;

		timeInSunlight = Mathf.Max(0f, timeInSunlight);
		if (timeInSunlight.Approximately(0f))
		{
			Reset();
			return;
		}

		SetVisuals();

		if (timeInSunlight >= maxTimeInSunlight)
			Die();
	}

	private void Reset()
	{

	}

	private void SetVisuals()
	{
		fill.fillAmount = 1 - (timeInSunlight / maxTimeInSunlight);
	}

	private void Die()
	{
		Debug.Log("Die!");
		controller.OnDie();

		StartCoroutine(ScaleDown());
		IEnumerator ScaleDown()
		{
			var elapsed = 0f;
			var dur = 1f;

			while (elapsed < dur)
			{
				elapsed += Time.deltaTime;
				transform.localScale = Vector3.one * (1 - (elapsed / dur));
				yield return null;
			}
		}
	}
}
