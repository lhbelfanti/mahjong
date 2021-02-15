using System.Collections.Generic;
using Board.Tile;
using Level;
using UnityEngine;

namespace Board
{
	public class BoardInfo
	{
		private Vector3 _boardSize;
		public void GetBoardSpecs(List<LevelInfo> levelInfo, out int tilesCount)
		{
			int tc = 0;
			int rows = 0;
			int cols = 0;
			int floors = 0;
			int vertical = 0;
			for (int i = 0; i < levelInfo.Count; i++)
			{
				List<LevelTiles> levelTiles = levelInfo[i].rows;
				for (int j = 0; j < levelTiles.Count; j++)
				{
					List<int> tiles = levelTiles[j].tiles;
					for (int k = 0; k < tiles.Count; k++)
					{
						if (tiles[k] != (int) TileCreator.TileTypes.Empty)
							tc++;

						if (tiles[k] == (int) TileCreator.TileTypes.DoubleH)
							k++;

						if (tiles[k] == (int) TileCreator.TileTypes.DoubleV && vertical == 0)
						{
							vertical++;
							tc++;
						}
						else if (tiles[k] == (int) TileCreator.TileTypes.DoubleV && vertical != 0)
						{
							vertical--;
						}
					}

					if (rows < tiles.Count)
						rows = tiles.Count;
				}

				if (cols < levelTiles.Count)
					cols = levelTiles.Count;

				floors++;
			}

			tilesCount = tc;
			_boardSize = new Vector3(rows, cols, floors);
		}

		public Vector3 BoardSize => _boardSize;
	}
}
