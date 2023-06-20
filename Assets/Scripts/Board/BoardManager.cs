using Level;
using Scene;
using UnityEngine;

namespace Board
{
	public class BoardManager : MonoBehaviour
	{
		[SerializeField] private Camera mainCamera;
		[SerializeField] private Vector2 fovForHeight;
		[SerializeField] private int fovGap;
		[SerializeField] private Transform boardGameObject;
		[SerializeField] private AutoAdjustBackground backgroundAdjuster;

		private BoardCreator _boardCreator;

		private void Awake()
		{
			_boardCreator = GetComponent<BoardCreator>();
		}

		private void Start()
		{
			int levelSelected = PlayerPrefs.GetInt("LevelSelected");
#if (UNITY_WEBGL)
			_boardCreator.LoadLevelWebGL(levelSelected);
#else
			LevelData levelData = _boardCreator.LoadLevel(levelSelected);
			_boardCreator.CreateBoard(levelData);
#endif
			CenterCameraOnBoard();
		}

		private void CenterCameraOnBoard()
		{
			Transform cameraTransform = mainCamera.transform;
			Vector3 boardPosition = boardGameObject.position;
			cameraTransform.position = new Vector3(boardPosition.x, boardPosition.y, cameraTransform.position.z);
			mainCamera.transform.LookAt(boardGameObject, Vector3.forward);

			int boardSizeY = (int) _boardCreator.BoardSize.y;
			Tile.Tile t = _boardCreator.MiddleTile;
			Rect tileRect = t.GetComponent<RectTransform>().rect;
			Vector3 tilePos = t.transform.position;
			float newBoardPosX = tilePos.x;
			float newBoardPosY = tilePos.y + (boardSizeY % 2 == 0 ? tileRect.height / 2 : 0);
			boardGameObject.position = new Vector3(-newBoardPosX, -newBoardPosY, boardPosition.z);

			int fov = (int) fovForHeight.x;
			int boardHeight = (int) fovForHeight.y;
			int newFov = fov - (fov - boardSizeY * fov / boardHeight) / 2;
			mainCamera.fieldOfView = newFov + fovGap;

			_boardCreator.RemoveMiddleTile();

			backgroundAdjuster.AdjustBackground();
		}
	}
}
