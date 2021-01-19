using System;
using UnityEngine;

namespace MainMenu
{
	public class MainMenu : MonoBehaviour
	{
		private AudioManager _audioManager;

		private void Start()
		{
			_audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		}

		public void PlayButton()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
		}

		public void OptionsButton()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
		}

		public void QuitGame()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
			Application.Quit();
		}
	}
}
