using System.Collections.Generic;
using UnityEngine;

public class BoardInfo
{
	private Vector3 _boardSize;
	public void GetBoardSpecs(List<LevelInfo> levelInfo, out int tilesCount)
	{
		int tc = 0;
		int rows = 0;
		int cols = 0;
		int floors = 0;
		for (int i = 0; i < levelInfo.Count; i++)
		{
			List<LevelTiles> levelTiles = levelInfo[i].rows;
			for (int j = 0; j < levelTiles.Count; j++)
			{
				List<int> tiles = levelTiles[j].tiles;
				for (int k = 0; k < tiles.Count; k++)
				{
					if (tiles[k] != (int) Tile.States.Empty)
						tc++;

					if (tiles[k] == (int) Tile.States.Double)
						k++;
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
