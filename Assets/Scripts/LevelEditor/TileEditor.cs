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

		public GameObject CreateTile(TileCreator.TileStates id, int selectedFloor, Vector2 boardSize)
		{
			_selectedFloor = selectedFloor;
			_boardSize = boardSize;

			GameObject tile = null;
			switch (id)
			{
				case TileCreator.TileStates.Single:
					tile = InstantiateTile(TileCreator.TileStates.Single);
					break;
				case TileCreator.TileStates.DoubleH:
					tile = InstantiateTile(TileCreator.TileStates.DoubleH, gridCell.rect.width / 2 + GridEditor.Gap);
					break;
				case TileCreator.TileStates.DoubleV:
					tile = InstantiateTile(TileCreator.TileStates.DoubleV, 0f, -(gridCell.rect.height / 2 + GridEditor.Gap));
					break;
			}

			return tile;
		}

		private GameObject InstantiateTile(TileCreator.TileStates state, float xOffset = 0f, float yOffset = 0f)
		{
			GameObject floor = GameObject.Find($"Floor {_selectedFloor.ToString()}");
			GameObject tile = Instantiate(boardTile, floor.transform, true);
			Vector3 tilePos = tile.transform.position;
			tile.transform.position = new Vector3(tilePos.x + xOffset, tilePos.y + yOffset, tilePos.z);

			Snap snapScript = tile.GetComponent<Snap>();
			snapScript.EditorTile = tile.GetComponent<EditorTile>();
			snapScript.EditorTile.floor = _selectedFloor;
			snapScript.EditorTile.state = state;
			snapScript.GridCell = gridCell;
			snapScript.BoardSize(_boardSize);


			Selection.activeGameObject = tile;

			return tile;
		}
	}
}
