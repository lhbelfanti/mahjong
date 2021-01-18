using System;
using UnityEngine;

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

	private GameState _gameState;

	private void Awake()
	{
		_gameState = GameState.Default;
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

	}

	private void EndGame()
	{

	}

	public void SwitchGameState(GameState gameState)
	{
		_gameState = gameState;
	}
}
