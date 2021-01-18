using UnityEngine;
using UnityEngine.Audio;

namespace MainMenu
{
	public class OptionsMenu : MonoBehaviour
	{
		[SerializeField] private AudioMixer audioMixer;

		public void SetVolume(float volume)
		{
			audioMixer.SetFloat("volume", volume);
		}
	}
}
