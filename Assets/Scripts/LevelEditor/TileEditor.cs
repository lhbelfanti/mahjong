using Board.Tile;
using UnityEditor;
using UnityEngine;

namespace LevelEditor
{
	public class TileEditor : MonoBehaviour
	{
		[SerializeField] private GameObject boardTile;

		private int _selectedFloor;
		private Vector2 _boardSize;

		public void CreateTile(int id, int selectedFloor, Vector2 boardSize)
		{
			_selectedFloor = selectedFloor;
			_boardSize = boardSize;

			switch (id)
			{
				case (int) TileCreator.TileStates.Single:
					InstantiateAndSetState(TileCreator.TileStates.Single);
					break;
				case (int) TileCreator.TileStates.DoubleH:
					InstantiateAndSetState(TileCreator.TileStates.DoubleH);
					break;
				case (int) TileCreator.TileStates.DoubleV:
					InstantiateAndSetState(TileCreator.TileStates.DoubleV);
					break;
			}
		}

		private void InstantiateAndSetState(TileCreator.TileStates state)
		{
			GameObject floor = GameObject.Find($"Floor {_selectedFloor.ToString()}");
			GameObject tile = Instantiate(boardTile, floor.transform, true);
			Snap snapScript = tile.GetComponent<Snap>();
			snapScript.TileState = state;
			snapScript.Renamer = tile.GetComponent<Renamer>();
			snapScript.BoardSize(_boardSize);

			Selection.activeGameObject = tile;
		}
	}
}
