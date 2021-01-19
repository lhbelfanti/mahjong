using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
	public class OptionsMenu : MonoBehaviour
	{
		[SerializeField] private Slider slider;

		private AudioManager _audioManager;

		private void Start()
		{
			_audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
			slider.value = _audioManager.GetVolume();
		}

		public void SetVolume(float volume)
		{
			_audioManager.SetVolume(volume);
		}

		public void OnBackClicked()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
			_audioManager.SaveCurrentVolume();
		}
	}
}
