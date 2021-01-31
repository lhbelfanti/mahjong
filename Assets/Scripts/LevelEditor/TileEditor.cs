using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class TileEditor : MonoBehaviour
	{
		[SerializeField] private GameObject boardTile;

		public void CreateTile(int id)
		{
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
			GameObject tile = Instantiate(boardTile);
			Snap snapScript = tile.GetComponent<Snap>();
			snapScript.tileState = state;
		}
	}
}
