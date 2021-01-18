using UnityEngine;

public class WinLoseUIManager : MonoBehaviour
{
	[SerializeField] private SceneFader _sceneFader;

	public void Retry()
	{
		gameObject.SetActive(false);
		_sceneFader.FadeTo("Game");
	}

	public void Next()
	{
		gameObject.SetActive(false);
		_sceneFader.FadeTo("Game");
	}

	public void Menu()
	{
		gameObject.SetActive(false);
		_sceneFader.FadeTo("Main");
	}
}
