using Audio;
using TMPro;
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
		[SerializeField] private GameObject winLastLevelUI;
		[SerializeField] private TextMeshProUGUI levelText;

		private AudioManager _audioManager;

		private GameState _gameState;

		private void Start()
		{
			_gameState = GameState.Default;
			_audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
			levelText.text += PlayerPrefs.GetInt("LevelSelected", 1).ToString();
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

			int currentLevel = PlayerPrefs.GetInt("LevelSelected", 1);
			int lastLevel = PlayerPrefs.GetInt("LastLevel");

			if (currentLevel == lastLevel)
				winLastLevelUI.SetActive(true);
			else
				winUI.SetActive(true);

			PlayerPrefs.SetInt("LastUnlockedLevel", PlayerPrefs.GetInt("LastUnlockedLevel", 1) + 1);
			PlayerPrefs.SetInt("LevelSelected", currentLevel + 1);
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
