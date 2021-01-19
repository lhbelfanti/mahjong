using UnityEngine;

namespace Scene
{
	public class DontDestroyAudio : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(transform.gameObject);
		}
	}
}
