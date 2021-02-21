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
		private GridEditor _gridEditor;

		public GameObject CreateTile(TileCreator.TileTypes type, int selectedFloor, int x = 0, int y = 0)
		{
			_selectedFloor = selectedFloor;
			_gridEditor = transform.GetComponent<GridEditor>();

			Rect rect = gridCell.rect;
			float xOffset = x * (rect.width + GridEditor.Gap);
			float yOffset = y * (rect.height + GridEditor.Gap);

			GameObject tile = null;
			switch (type)
			{
				case TileCreator.TileTypes.Single:
					tile = InstantiateTile(TileCreator.TileTypes.Single, x, y, xOffset, -yOffset);
					break;
				case TileCreator.TileTypes.DoubleH:
					tile = InstantiateTile(TileCreator.TileTypes.DoubleH, x, y, xOffset + rect.width / 2, -yOffset);
					break;
				case TileCreator.TileTypes.DoubleV:
					tile = InstantiateTile(TileCreator.TileTypes.DoubleV, x, y, xOffset, -(yOffset + rect.height / 2));
					break;
			}

			return tile;
		}

		private GameObject InstantiateTile(TileCreator.TileTypes type, int x, int y, float xOffset = 0f, float yOffset = 0f)
		{
			GameObject floor = GameObject.Find($"Floor {_selectedFloor.ToString()}");
			GameObject tile = Instantiate(boardTile, floor.transform, true);
			Vector3 tilePos = tile.transform.position;
			tile.transform.position = new Vector3(tilePos.x + xOffset, tilePos.y + yOffset, -_selectedFloor);

			EditorTile editorTile = tile.GetComponent<EditorTile>();
			editorTile.GridEditor = _gridEditor;
			// Made on purpose
			editorTile.x = y;
			editorTile.y = x;
			editorTile.floor = _selectedFloor;
			editorTile.type = type;
			editorTile.SetName();

			Selection.activeGameObject = tile;

			return tile;
		}
	}
}
