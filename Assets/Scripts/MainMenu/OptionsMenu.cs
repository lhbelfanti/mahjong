using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
	public class OptionsMenu : MonoBehaviour
	{
		[SerializeField] private Slider musicSlider;
		[SerializeField] private Slider fxSlider;

		private AudioManager _audioManager;

		private void Start()
		{
			_audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
			musicSlider.value = _audioManager.GetMusicVolume();
			fxSlider.value = _audioManager.GetFXVolume();
		}

		public void SetMusicVolume(float volume)
		{
			_audioManager.SetMusicVolume(volume);
		}

		public void SetFXVolume(float volume)
		{
			_audioManager.SetFXVolume(volume);
		}

		public void OnBackClicked()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
			_audioManager.SaveCurrentVolume();
		}
	}
}
