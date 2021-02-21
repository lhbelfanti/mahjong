using System.IO;
using Audio;
using Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
	public class LevelButtonsLoader : MonoBehaviour
	{
		[SerializeField] private GameObject levelButton;
		[SerializeField] private GameObject lockedLevelButton;
		[SerializeField] private SceneFader sceneFader;

		private AudioManager _audioManager;
		private int _lastUnlockedLevel;

		private void Start()
		{
			_audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
			_lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);
			CreateLevelButtons();
		}

		private void CreateLevelButtons()
		{
			DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Text/");
			FileInfo[] info = dir.GetFiles("level*.json");
			int availableLevels = info.Length;

			PlayerPrefs.SetInt("LastLevel", availableLevels);

			for (int i = 0; i < availableLevels; i++)
			{
				int levelId = i + 1;
				CreateButton(levelId,levelId > _lastUnlockedLevel);
			}
		}

		private void CreateButton(int index, bool locked = false)
		{
			GameObject lvlGameObject = !locked ? levelButton : lockedLevelButton;
			GameObject lvlButton = Instantiate(lvlGameObject, gameObject.transform, true);
			lvlButton.GetComponentInChildren<TextMeshProUGUI>().text += index.ToString();
			lvlButton.name = $"Level {index.ToString()}";
			if (!locked)
				lvlButton.GetComponent<Button>().onClick.AddListener(() => OnLevelButtonClicked(index));
		}

		private void OnLevelButtonClicked(int levelId)
		{
			_audioManager.PlaySound(_audioManager.buttonClicked);
			PlayerPrefs.SetInt("LevelSelected", levelId);
			sceneFader.FadeTo("Game");
		}
	}
}
