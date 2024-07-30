using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float sunlightDamageSpeed;
	[SerializeField] private float maxTimeInSunlight;
	[SerializeField] private float damageEffectThreshold;

	[SerializeField] private Volume normalVolume;
	[SerializeField] private Volume damageVolume;

	private PlayerController controller;
	private float timeInSunlight;

	public void Init(PlayerController controller)
	{
		this.controller = controller;
	}

	private void Awake()
	{
		normalVolume.weight = 1f;
		damageVolume.weight = 0f;
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
			timeInSunlight -= sunlightDamageSpeed * Time.deltaTime * 2f;

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
		if (timeInSunlight <= damageEffectThreshold)
		{
			// If the time in sunlight is less than or equal to the threshold, weight is 0
			damageVolume.weight = 0f;
		}
		else
		{
			// If the time in sunlight is greater than the threshold, calculate the weight
			var adjustedTimeInSunlight = timeInSunlight - damageEffectThreshold;
			var adjustedMaxTime = maxTimeInSunlight - damageEffectThreshold;
			damageVolume.weight = Mathf.Clamp01(adjustedTimeInSunlight / adjustedMaxTime);
		}
	}

	private void Die()
	{
		Debug.Log("Die!");
		controller.OnDie();
	}
}
