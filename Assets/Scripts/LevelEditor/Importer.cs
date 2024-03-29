﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Board;
using Board.Tile;
using Level;
using UnityEngine;

namespace LevelEditor
{
	public class Importer
	{
		private readonly BoardInfo _boardInfo;
		private readonly GridEditor _gridEditor;
		private readonly BoardEditor _boardEditor;
		private readonly FloorEditor _floorEditor;
		private readonly TileEditor _tileEditor;

		public Importer(GridEditor gridEditor, BoardEditor boardEditor, FloorEditor floorEditor, TileEditor tileEditor)
		{
			_boardInfo = new BoardInfo();
			_gridEditor = gridEditor;
			_boardEditor = boardEditor;
			_floorEditor = floorEditor;
			_tileEditor = tileEditor;
		}
		public void ImportLevel(string path)
		{
			string[] splitPath = Regex.Split(path, @"/Resources/")[1].Split('.');
			TextAsset levelJson = Resources.Load<TextAsset>(splitPath[0]);
			LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
			List<LevelInfo> levelInfo = levelData.data;

			_boardInfo.GetBoardSpecs(levelInfo, out int tc);
			_gridEditor.Width = (int) _boardInfo.BoardSize.x;
			_gridEditor.Height = (int) _boardInfo.BoardSize.y;
			_gridEditor.CreateGrid();

			for (int i = 0; i < levelInfo.Count; i++)
			{
				int f = levelInfo[i].floor;
				_floorEditor.AddNewFloor();
				List<LevelTiles> levelTiles = levelInfo[i].rows;
				for (int j = 0; j < levelTiles.Count; j++)
				{
					List<int> tiles = levelTiles[j].tiles;
					for (int k = 0; k < tiles.Count; k++)
					{
						TileCreator.TileTypes type = (TileCreator.TileTypes) tiles[k];
						if (type == TileCreator.TileTypes.Empty ||
						    type == TileCreator.TileTypes.DummyH ||
						    type == TileCreator.TileTypes.DummyV)
							continue;
						GameObject tile = _tileEditor.CreateTile(type, f, k, j);
						EditorTile editorTile = tile.GetComponent<EditorTile>();
						_boardEditor.AddTile(editorTile);
					}
				}
			}
		}
	}
}
