using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public AudioMixer audioMixer;
	public AudioSource soundsAudioSource;

	[Header("Main music")]
	public AudioClip music;

	[Header("UI buttons")]
	public AudioClip buttonClicked;

	[Header("Popups")]
	public AudioClip win;
	public AudioClip lose;

	[Header("Tile")]
	public AudioClip wrongMatch;
	public AudioClip validMatch;

	private AudioSource _musicAudioSource;

	private void Awake()
	{
		_musicAudioSource = gameObject.GetComponent<AudioSource>();
		_musicAudioSource.clip = music;
		_musicAudioSource.Play();
		SetVolume(PlayerPrefs.GetFloat("Volume"));
	}

	public void PlaySound(AudioClip audioClip)
	{
		soundsAudioSource.clip = audioClip;
		soundsAudioSource.Play();
	}

	public void SetVolume(float value)
	{
		_musicAudioSource.volume = value;
		soundsAudioSource.volume = value;
	}

	public void SaveCurrentVolume()
	{
		PlayerPrefs.SetFloat("Volume", _musicAudioSource.volume);
	}

	public float GetVolume()
	{
		return PlayerPrefs.GetFloat("Volume");
	}

	public void PauseMusic()
	{
		_musicAudioSource.Pause();
	}

	public void ResumeMusic()
	{
		_musicAudioSource.UnPause();
	}
}
