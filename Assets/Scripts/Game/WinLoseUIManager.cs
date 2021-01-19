using System;
using Scene;
using UnityEngine;

namespace Game
{
	public class WinLoseUIManager : MonoBehaviour
	{
		[SerializeField] private SceneFader sceneFader;

		private AudioManager _audioManager;

		private void Awake()
		{
			_audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		}

		public void Retry()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
			gameObject.SetActive(false);
			sceneFader.FadeTo("Game");
			_audioManager.ResumeMusic();
		}

		public void Next()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
			gameObject.SetActive(false);
			sceneFader.FadeTo("Game");
			_audioManager.ResumeMusic();
		}

		public void Menu()
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
			gameObject.SetActive(false);
			sceneFader.FadeTo("Main");
			_audioManager.ResumeMusic();
		}
	}
}
