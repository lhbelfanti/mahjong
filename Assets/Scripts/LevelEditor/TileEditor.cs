using Board.Tile;
using UnityEditor;
using UnityEngine;

namespace LevelEditor
{
	public class TileEditor : MonoBehaviour
	{
		[SerializeField] private GameObject boardTile;
		[SerializeField] private RectTransform gridCell;

		private int _selectedFloor;
		private Vector2 _boardSize;

		public void CreateTile(int id, int selectedFloor, Vector2 boardSize)
		{
			_selectedFloor = selectedFloor;
			_boardSize = boardSize;

			switch (id)
			{
				case (int) TileCreator.TileStates.Single:
					InstantiateTile(TileCreator.TileStates.Single);
					break;
				case (int) TileCreator.TileStates.DoubleH:
					InstantiateTile(TileCreator.TileStates.DoubleH, gridCell.rect.width / 2 + GridEditor.Gap);
					break;
				case (int) TileCreator.TileStates.DoubleV:
					InstantiateTile(TileCreator.TileStates.DoubleV, 0f, -(gridCell.rect.height / 2 + GridEditor.Gap));
					break;
			}
		}

		private void InstantiateTile(TileCreator.TileStates state, float xOffset = 0f, float yOffset = 0f)
		{
			GameObject floor = GameObject.Find($"Floor {_selectedFloor.ToString()}");
			GameObject tile = Instantiate(boardTile, floor.transform, true);
			Vector3 tilePos = tile.transform.position;
			tile.transform.position = new Vector3(tilePos.x + xOffset, tilePos.y + yOffset, tilePos.z);

			Snap snapScript = tile.GetComponent<Snap>();
			snapScript.GridCell = gridCell;
			snapScript.TileState = state;
			snapScript.Renamer = tile.GetComponent<Renamer>();
			snapScript.BoardSize(_boardSize);

			Selection.activeGameObject = tile;
		}
	}
}
