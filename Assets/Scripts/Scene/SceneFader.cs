using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

namespace Scene
{
	public class SceneFader : MonoBehaviour
	{
		[SerializeField] private Image img;
		[SerializeField] private AnimationCurve animationCurve;

		private void Start()
		{
			StartCoroutine(FadeIn());
		}

		public void FadeTo(string scene)
		{
			StartCoroutine(FadeOut(scene));
		}

		private IEnumerator FadeIn()
		{
			float t = 1f;
			Color imgColor = img.color;

			while (t > 0f)
			{
				t -= Time.deltaTime * 1f;
				float a = animationCurve.Evaluate(t);
				img.color = new Color(imgColor.r, imgColor.g, imgColor.b, a);
				yield return 0;
			}
		}

		private IEnumerator FadeOut(string scene)
		{
			float t = 0f;
			Color imgColor = img.color;

			while (t < 1f)
			{
				t += Time.deltaTime * 1f;
				float a = animationCurve.Evaluate(t);
				img.color = new Color(imgColor.r, imgColor.g, imgColor.b, a);
				yield return 0;
			}

			SceneManager.LoadScene(scene);
		}
	}
}
