using System.Collections.Generic;
using Board.Tile;
using Level;
using UnityEngine;
using Utils;

namespace LevelEditor
{
	public class Exporter
	{
		private int[,,] _tiles;
		private readonly Vector3 _dimensions;
		private readonly List<EditorTile> _editorTiles;

		public bool canBeExported;

		public Exporter(List<EditorTile> editorTiles, Vector3 dimensions)
		{
			_dimensions = dimensions;
			_editorTiles = editorTiles;
		}

		public void Validate()
		{
			Utils.Utils.ClearConsole();
			canBeExported = true;

			if (_editorTiles.Count % 2 != 0)
			{
				canBeExported = false;
				Debug.LogError("The number of tiles must be even.");
			}

			_dimensions.ToInts(out int width, out int height, out int floors);
			_tiles = new int[width, height, floors];

			foreach (EditorTile et in _editorTiles)
			{
				ITileValidator tileValidator;
				switch (et.state)
				{
					case TileCreator.TileStates.Single:
						tileValidator = new SingleTileValidator(_tiles, et);
						if (!tileValidator.Validate())
							canBeExported = false;

						_tiles[et.x, et.y, et.floor] = 1;
						break;
					case TileCreator.TileStates.DoubleH:
						tileValidator = new DoubleHTileValidator(_tiles, et);
						if (!tileValidator.Validate())
							canBeExported = false;

						_tiles[et.x, et.y, et.floor] = 2;
						_tiles[et.x + 1, et.y, et.floor] = 2;
						break;
					case TileCreator.TileStates.DoubleV:
						tileValidator = new DoubleVTileValidator(_tiles, et);
						if (!tileValidator.Validate())
							canBeExported = false;

						_tiles[et.x, et.y, et.floor] = 4;
						_tiles[et.x, et.y + 1, et.floor] = 4;
						break;
				}
			}

			if (canBeExported)
				Debug.Log("Validation Successful!");
		}

		public void SaveTemp()
		{
			_dimensions.ToInts(out int width, out int height, out int floors);
			LevelData levelData = new LevelData();
			levelData.fillMethod = 0; //TODO: Change it
			levelData.data = new List<LevelInfo>();

			for (int f = 0; f < floors; f++)
			{
				LevelInfo levelInfo = new LevelInfo();
				levelInfo.floor = f;
				levelInfo.rows = new List<LevelTiles>();
				for (int w = 0; w < width; w++)
				{
					LevelTiles levelTiles = new LevelTiles();
					levelTiles.tiles = new List<int>();
					for (int h = 0; h < height; h++)
					{
						levelTiles.tiles.Add(_tiles[w, h, f]);
					}

					levelInfo.rows.Add(levelTiles);
				}

				levelData.data.Add(levelInfo);
			}

			string levelDataJson = JsonUtility.ToJson(levelData);

		}
	}
}
