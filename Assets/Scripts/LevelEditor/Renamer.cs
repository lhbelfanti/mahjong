using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class Renamer : MonoBehaviour
	{
		public void Rename(TileCreator.TileStates tileState, int x, int y)
		{
			string newName = $"{y.ToString()}x{x.ToString()} - ";
			switch (tileState)
			{
				case TileCreator.TileStates.Single:
					newName += "Single";
					break;
				case TileCreator.TileStates.DoubleH:
					newName += "DoubleH";
					break;
				case TileCreator.TileStates.DoubleV:
					newName += "DoubleV";
					break;
			}

			transform.name = newName;
		}
	}
}
