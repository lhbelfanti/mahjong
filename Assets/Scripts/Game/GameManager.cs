﻿using UnityEngine;

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
		PlayerPrefs.SetInt("LastUnlockedLevel", PlayerPrefs.GetInt("LastUnlockedLevel", 1) + 1);
		winUI.SetActive(true);
	}

	private void EndGame()
	{
		gameOverUI.SetActive(true);
	}

	public void SwitchGameState(GameState gameState)
	{
		_gameState = gameState;
	}
}