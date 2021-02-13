using Audio;
using UnityEngine;

namespace Game
{
	public class GameManager : MonoBehaviour
	{
		public enum GameState
		{
			Default = 0,
			Win = 1,
			Lose = 2
		}

		[SerializeField] private GameObject gameOverUI;
		[SerializeField] private GameObject winUI;

		private AudioManager _audioManager;

		private GameState _gameState;

		private void Start()
		{
			_gameState = GameState.Default;
			_audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		}

		private void Update()
		{
			switch (_gameState)
			{
				case GameState.Default:
					break;
				case GameState.Win:
					WinLevel();
					break;
				case GameState.Lose:
					EndGame();
					break;
			}

			_gameState = GameState.Default;
		}

		private void WinLevel()
		{
			_audioManager.PauseMusic();
			_audioManager.PlaySound(_audioManager.win);
			PlayerPrefs.SetInt("LastUnlockedLevel", PlayerPrefs.GetInt("LastUnlockedLevel", 1) + 1);
			PlayerPrefs.SetInt("LevelSelected", PlayerPrefs.GetInt("LevelSelected", 1) + 1);
			winUI.SetActive(true);
		}

		private void EndGame()
		{
			_audioManager.PauseMusic();
			_audioManager.PlaySound(_audioManager.lose);
			gameOverUI.SetActive(true);
		}

		public void SwitchGameState(GameState gameState)
		{
			_gameState = gameState;
		}
	}
}
