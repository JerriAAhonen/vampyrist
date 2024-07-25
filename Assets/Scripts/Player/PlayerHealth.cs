using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float sunlightDamageSpeed;
	[SerializeField] private float maxTimeInSunlight;
	[SerializeField] private Image fill;

	private float timeInSunlight;

	private void Update()
	{
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
		Debug.Log(timeInSunlight);
		fill.fillAmount = 1 - (timeInSunlight / maxTimeInSunlight);
	}

	private void Die()
	{
		Debug.Log("Die!");
	}
}
