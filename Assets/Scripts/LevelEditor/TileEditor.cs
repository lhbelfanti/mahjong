using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class TileEditor : MonoBehaviour
	{
		[SerializeField] private GameObject boardTile;

		public void CreateTile(int id, int selectedFloor, Vector2 boardSize)
		{
			switch (id)
			{
				case (int) TileCreator.TileStates.Single:
					InstantiateAndSetState(TileCreator.TileStates.Single, selectedFloor, boardSize);
					break;
				case (int) TileCreator.TileStates.DoubleH:
					InstantiateAndSetState(TileCreator.TileStates.DoubleH, selectedFloor, boardSize);
					break;
				case (int) TileCreator.TileStates.DoubleV:
					InstantiateAndSetState(TileCreator.TileStates.DoubleV, selectedFloor, boardSize);
					break;

			}
		}

		private void InstantiateAndSetState(TileCreator.TileStates state, int selectedFloor, Vector2 boardSize)
		{
			GameObject floor = GameObject.Find($"Floor {selectedFloor.ToString()}");
			GameObject tile = Instantiate(boardTile, floor.transform, true);
			Snap snapScript = tile.GetComponent<Snap>();
			snapScript.TileState = state;
			snapScript.BoardSize(boardSize);

		}
	}
}
