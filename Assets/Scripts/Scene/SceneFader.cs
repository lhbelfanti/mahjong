using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using Image = UnityEngine.UI.Image;

public class SceneFader : MonoBehaviour
{
	[SerializeField] private Image _img;
	[SerializeField] private AnimationCurve _animationCurve;

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
		Color imgColor = _img.color;

		while (t > 0f)
		{
			t -= Time.deltaTime * 1f;
			float a = _animationCurve.Evaluate(t);
			_img.color = new Color(imgColor.r, imgColor.g, imgColor.b, a);
			yield return 0;
		}
	}

	private IEnumerator FadeOut(string scene)
	{
		float t = 0f;
		Color imgColor = _img.color;

		while (t < 1f)
		{
			t += Time.deltaTime * 1f;
			float a = _animationCurve.Evaluate(t);
			_img.color = new Color(imgColor.r, imgColor.g, imgColor.b, a);
			yield return 0;
		}

		SceneManager.LoadScene(scene);
	}
}
