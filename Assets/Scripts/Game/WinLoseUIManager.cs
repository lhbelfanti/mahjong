using Scene;
using UnityEngine;

namespace Game
{
	public class WinLoseUIManager : MonoBehaviour
	{
		[SerializeField] private SceneFader sceneFader;

		public void Retry()
		{
			gameObject.SetActive(false);
			sceneFader.FadeTo("Game");
		}

		public void Next()
		{
			gameObject.SetActive(false);
			sceneFader.FadeTo("Game");
		}

		public void Menu()
		{
			gameObject.SetActive(false);
			sceneFader.FadeTo("Main");
		}
	}
}
