using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class AudioManager : PersistentSingleton<AudioManager>
{
	public const string PPKey_VolumeOn = "VolumeOn";

	[SerializeField] private Transform sourceContainer;
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioMixerGroup audioMixerGroup;
	[SerializeField] private AudioClip music;
	[SerializeField] private float musicVolume;

	private IObjectPool<AudioSource> pool;
	private List<AudioSource> activeSources;
	private Dictionary<AudioEvent, float> cooldowns;

	private AudioSource musicSource;

	protected override void Awake()
	{
		base.Awake();
			
		PoolUtil.Get(ref activeSources);
		PoolUtil.Get(ref cooldowns);
			
		pool = new ObjectPool<AudioSource>(
			CreateAudioSource,
			s =>
			{
				s.gameObject.SetActive(true);
				activeSources.Add(s);
			},
			s =>
			{
				s.Stop();
				s.clip = null;
				s.gameObject.SetActive(false);
				s.transform.SetParent(sourceContainer);
				activeSources.Remove(s);
			},
			Destroy);

		musicSource = CreateAudioSource();
		musicSource.clip = music;
		musicSource.volume = musicVolume;
		musicSource.loop = true;
		musicSource.transform.SetParent(sourceContainer);
		musicSource.transform.localPosition = Vector3.zero;
		musicSource.Play();
		musicSource.outputAudioMixerGroup = audioMixerGroup;
	}

	private void LateUpdate()
	{
		foreach (var s in activeSources)
		{
			if (!s.isPlaying)
			{
				pool.Release(s);
				return;
			}
		}
	}

	private void OnDisable()
	{
		PoolUtil.Release(ref activeSources);
		PoolUtil.Release(ref cooldowns);
	}

	public void ToggleVolume()
	{
		var vol = PlayerPrefs.GetInt(PPKey_VolumeOn, 1) == 1f
			? 0.00001f
			: 1f;

		audioMixer.SetFloat("Volume", Mathf.Log10(vol) * 20);
		PlayerPrefs.SetInt(PPKey_VolumeOn, Mathf.RoundToInt(vol));
	}

	/// <summary>
	/// Plays given Audio event once
	/// </summary>
	/// <param name="ae">Audio Event</param>
	/// <returns>Duration of the audio clip</returns>
	public float PlayOnce(AudioEvent ae)
	{
		if (!CanBePlayed(ae))
			return 0f;
			
		var s = pool.Get();
		s.clip = ae.Clip;
		s.volume = ae.Volume;
		s.pitch = ae.Pitch;
		s.loop = false;
		s.outputAudioMixerGroup = audioMixerGroup;

		if (ae.Delay <= 0f)
			s.Play();
		else
			s.PlayDelayed(ae.Delay);
			
		return s.clip.length / Mathf.Abs(s.pitch);
	}

	private AudioSource CreateAudioSource()
	{
		var go = new GameObject("AudioSource");
		go.transform.SetParent(sourceContainer);
		go.transform.localPosition = Vector3.zero;

		var source = go.AddComponent<AudioSource>();
		source.playOnAwake = false;
		source.spatialBlend = 0f;
		source.minDistance = 10f;
		source.maxDistance = 30f;
		return source;
	}

	private bool CanBePlayed(AudioEvent ae)
	{
		if (ae.MINInterval <= 0f)
			return true;

		if (cooldowns.TryGetValue(ae, out float endsAt) && Time.realtimeSinceStartup < endsAt)
			return false;

		cooldowns[ae] = Time.realtimeSinceStartup + ae.MINInterval;
		return true;
	}
}