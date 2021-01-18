using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class GameOver : MonoBehaviour
{
	[SerializeField] private Text _titleText;
	[SerializeField] private Button _retryButton;
	[SerializeField] private Button _menuButton;
	[SerializeField] private SceneFader _sceneFader;

	public void Retry()
	{
		_sceneFader.FadeTo("Game");
	}

	public void Menu()
	{
		_sceneFader.FadeTo("Main");
	}
}
