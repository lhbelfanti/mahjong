using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
	public class AudioManager : MonoBehaviour
	{
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
			SetMusicVolume(GetMusicVolume());
			SetFXVolume(GetFXVolume());
			_musicAudioSource = gameObject.GetComponent<AudioSource>();
			_musicAudioSource.clip = music;
			_musicAudioSource.Play();
		}

		public void PlaySound(AudioClip audioClip)
		{
			soundsAudioSource.clip = audioClip;
			soundsAudioSource.Play();
		}

		public void SetMusicVolume(float value)
		{
			_musicAudioSource.volume = value;
		}

		public void SetFXVolume(float value)
		{
			soundsAudioSource.volume = value;
		}

		public void SaveCurrentVolume()
		{
			PlayerPrefs.SetFloat("MusicVolume", _musicAudioSource.volume);
			PlayerPrefs.SetFloat("FXVolume", _musicAudioSource.volume);
		}

		public float GetMusicVolume()
		{
			return PlayerPrefs.GetFloat("MusicVolume", 1);
		}

		public float GetFXVolume()
		{
			return PlayerPrefs.GetFloat("FXVolume", 1);
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
}
