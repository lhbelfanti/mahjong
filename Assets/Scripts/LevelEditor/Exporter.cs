using System.Collections.Generic;
using Board.Tile;
using Level;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

namespace LevelEditor
{
	public class Exporter
	{
		private int[,,] _tiles;
		private readonly List<EditorTile> _editorTiles;
		private readonly GridEditor _gridEditor;
		private readonly FloorEditor _floorEditor;

		public bool CanBeExported;

		public const int EditorLevelNum = 99999;
		public const string EditorLevelPath = "Text/editorLevel";


		public Exporter(List<EditorTile> editorTiles, GridEditor gridEditor, FloorEditor floorEditor)
		{
			_gridEditor = gridEditor;
			_floorEditor = floorEditor;
			_editorTiles = editorTiles;
		}

		public void Validate()
		{
			Utils.Utils.ClearConsole();
			CanBeExported = true;

			if (_editorTiles.Count % 2 != 0)
			{
				CanBeExported = false;
				Debug.LogError("The number of tiles must be even.");
			}

			_tiles = new int[_gridEditor.Height, _gridEditor.Width, _floorEditor.FloorsQuantity];

			foreach (EditorTile et in _editorTiles)
			{
				ITileValidator tileValidator;
				switch (et.type)
				{
					case TileCreator.TileTypes.Single:
						tileValidator = new SingleTileValidator(_tiles, et);
						if (!tileValidator.Validate())
							CanBeExported = false;

						_tiles[et.x, et.y, et.floor] = (int) TileCreator.TileTypes.Single;
						break;
					case TileCreator.TileTypes.DoubleH:
						tileValidator = new DoubleHTileValidator(_tiles, et);
						if (!tileValidator.Validate())
							CanBeExported = false;

						_tiles[et.x, et.y, et.floor] = (int) TileCreator.TileTypes.DoubleH;
						_tiles[et.x, et.y + 1, et.floor] = (int) TileCreator.TileTypes.DummyH;
						break;
					case TileCreator.TileTypes.DoubleV:
						tileValidator = new DoubleVTileValidator(_tiles, et);
						if (!tileValidator.Validate())
							CanBeExported = false;

						_tiles[et.x, et.y, et.floor] = (int) TileCreator.TileTypes.DoubleV;
						_tiles[et.x + 1, et.y, et.floor] = (int) TileCreator.TileTypes.DummyV;
						break;
				}
			}
		}

		public void SaveTemp(int fillType)
		{
			Save(EditorLevelNum, fillType, "");
			PlayerPrefs.SetInt("LevelSelected", 99999);
		}

		public void Save(int level, int fillType, string path)
		{
			LevelData levelData = new LevelData();
			levelData.level = level;
			levelData.fillMethod = fillType;
			levelData.data = new List<LevelInfo>();

			for (int f = 0; f < _floorEditor.FloorsQuantity; f++)
			{
				LevelInfo levelInfo = new LevelInfo();
				levelInfo.floor = f;
				levelInfo.rows = new List<LevelTiles>();
				for (int h = 0; h < _gridEditor.Height; h++)
				{
					LevelTiles levelTiles = new LevelTiles();
					levelTiles.tiles = new List<int>();
					for (int w = 0; w < _gridEditor.Width; w++)
					{
						levelTiles.tiles.Add(_tiles[h, w, f]);
					}

					levelInfo.rows.Add(levelTiles);
				}

				levelData.data.Add(levelInfo);
			}


			string url = path == ""
				? $"{Application.dataPath}/Resources/{EditorLevelPath}.json"
				: $"{path}level{level.ToString()}.json";

			JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented,};
			settings.Converters.Add(new CustomJsonConverter());
			string levelDataJson = JsonConvert.SerializeObject(levelData, settings);

			System.IO.File.WriteAllText(url, levelDataJson);
		}
	}
}
