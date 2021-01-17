using UnityEngine;

public class BoardManager : MonoBehaviour
{
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private Vector2 _fovForHeight;
	[SerializeField] private int _fovGap;
	[SerializeField] private Transform _boardGameObject;

	private BoardCreator _boardCreator;
	private BoardSelector _boardSelector;


	private void Awake()
	{
		_boardCreator = GetComponent<BoardCreator>();
	}

	private void Start()
	{
		int levelSelected = PlayerPrefs.GetInt("LevelSelected");
		_boardCreator.CreateBoard(levelSelected);
		CenterCameraOnBoard();
	}

	private void CenterCameraOnBoard()
	{
		Transform cameraTransform = _mainCamera.transform;
		Vector3 boardPosition = _boardGameObject.position;
		cameraTransform.position = new Vector3(boardPosition.x, boardPosition.y, cameraTransform.position.z);
		_mainCamera.transform.LookAt(_boardGameObject, Vector3.forward);

		int boardSizeY = (int) _boardCreator.BoardSize.y;
		Tile t = _boardCreator.MiddleTile;
		Rect tileRect = t.GetComponent<RectTransform>().rect;
		Vector3 tilePos = t.transform.position;
		float newBoardPosX = tilePos.x;
		float newBoardPosY = tilePos.y + (boardSizeY % 2 == 0 ? tileRect.height / 2 : 0);
		_boardGameObject.position = new Vector3(-newBoardPosX, -newBoardPosY, boardPosition.z);

		int fov = (int) _fovForHeight.x;
		int boardHeight = (int) _fovForHeight.y;
		int newFov = fov - (fov - boardSizeY * fov / boardHeight) / 2;
		_mainCamera.fieldOfView = newFov + _fovGap;

	}
}
