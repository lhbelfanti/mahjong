using UnityEngine;

namespace Scene
{
	public class AutoAdjustBackground : MonoBehaviour
	{
		[SerializeField] private Camera mainCamera;

		public void AdjustBackground()
		{
			Sprite sprite = GetComponent<SpriteRenderer>().sprite;
			float spriteHeight = sprite.bounds.size.y;
			float spriteWidth = sprite.bounds.size.x;
			float distance = transform.position.z - mainCamera.transform.position.z;
			float screenHeight = 2 * Mathf.Tan(mainCamera.fieldOfView * Mathf.Deg2Rad / 2) * distance;
			float screenWidth = screenHeight * mainCamera.aspect;
			transform.localScale = new Vector3(screenWidth / spriteWidth, screenHeight / spriteHeight, 1f);
		}
	}
}
