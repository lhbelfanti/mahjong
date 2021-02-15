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
		public TileCreator.TileTypes type;

		public string Name()
		{
			string newName = $"{x.ToString()}x{y.ToString()} - ";
			switch (type)
			{
				case TileCreator.TileTypes.Single:
					newName += "Single";
					break;
				case TileCreator.TileTypes.DoubleH:
					newName += "DoubleH";
					break;
				case TileCreator.TileTypes.DoubleV:
					newName += "DoubleV";
					break;
			}

			return newName;
		}
	}
}
