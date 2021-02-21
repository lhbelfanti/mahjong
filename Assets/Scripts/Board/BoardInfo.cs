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
			int totalTiles = 0;
			int rows = 0;
			int cols = 0;
			int floors = 0;

			foreach (LevelInfo lInfo in levelInfo)
			{
				List<LevelTiles> levelTiles = lInfo.rows;
				foreach (LevelTiles lTiles in levelTiles)
				{
					List<int> tiles = lTiles.tiles;
					foreach (int t in tiles)
					{
						TileCreator.TileTypes type = (TileCreator.TileTypes) t;

						if (type != TileCreator.TileTypes.Empty &&
						    type != TileCreator.TileTypes.DummyH &&
						    type != TileCreator.TileTypes.DummyV)
							totalTiles++;
					}
					rows = Mathf.Max(rows, tiles.Count);
				}
				cols = Mathf.Max(cols, levelTiles.Count);

				floors++;
			}

			tilesCount = totalTiles;
			_boardSize = new Vector3(rows, cols, floors);
		}

		public Vector3 BoardSize => _boardSize;
	}
}
