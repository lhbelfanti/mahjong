﻿using UnityEngine;

namespace Scene
{
	public class MainScene : MonoBehaviour
	{
		[SerializeField] private GameObject audioManager;

		private void Awake()
		{
			if (!GameObject.FindGameObjectWithTag("Audio"))
			{
				GameObject audioInstance = Instantiate(audioManager);
				audioInstance.name = "AudioManager";
			}
		}
	}
}
