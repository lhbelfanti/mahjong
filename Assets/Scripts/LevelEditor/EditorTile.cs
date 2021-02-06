using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class EditorTile : MonoBehaviour
	{
		[HideInInspector]
		public int x;
		[HideInInspector]
		public int y;
		[HideInInspector]
		public int floor;
		[HideInInspector]
		public TileCreator.TileStates state;

		public string Name()
		{
			string newName = $"{y.ToString()}x{x.ToString()} - ";
			switch (state)
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

			return newName;
		}
	}
}
