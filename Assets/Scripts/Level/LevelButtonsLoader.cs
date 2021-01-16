using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonsLoader : MonoBehaviour
{
	[SerializeField] private GameObject _levelButton;
	[SerializeField] private GameObject _lockedLevelButton;
	[SerializeField] private GameObject _levelsContainer;
	[SerializeField] private SceneFader _sceneFader;

	private int _lastUnlockedLevel = 0;

	private void Start()
	{
		_lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 2);
		CreateLevelButtons();
	}

	private void CreateLevelButtons()
	{
		DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Text/");
		FileInfo[] info = dir.GetFiles("level*.json");
		int availableLevels = info.Length;

		for (int i = 0; i < availableLevels; i++)
		{
			int levelId = i + 1;
			CreateButton(levelId,levelId > _lastUnlockedLevel);
		}
	}

	private void CreateButton(int index, bool locked = false)
	{
		GameObject lvlGameObject = !locked ? _levelButton : _lockedLevelButton;
		GameObject levelButton = Instantiate(lvlGameObject, _levelsContainer.transform, true);
		levelButton.GetComponentInChildren<TextMeshProUGUI>().text += index.ToString();;
		levelButton.name = $"Level {index.ToString()}";
		if (!locked)
			levelButton.GetComponent<Button>().onClick.AddListener(() => OnLevelButtonClicked(index));
	}

	private void OnLevelButtonClicked(int levelId)
	{
		PlayerPrefs.SetInt("LevelSelected", levelId);
		_sceneFader.FadeTo("Game");
	}
}
