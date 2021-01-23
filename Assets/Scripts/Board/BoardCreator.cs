using System;
using System.Collections.Generic;
using Board.Tile;
using Level;
using UnityEngine;
using Utils;
using Vector3 = UnityEngine.Vector3;
using TileStates = Board.Tile.TileCreator.TileStates;

namespace Board
{
	public class BoardCreator : MonoBehaviour
	{
		private BoardInfo _boardInfo;
		private BoardImages _boardImages;
		private Tile.Tile[,,] _boardTiles;
		private GameObject[] _floors;
		private Vector2 _middleTilePos;
		private Tile.Tile _middleTile;
		private TileCreator _tileCreator;

		private void Awake()
		{
			_boardInfo = new BoardInfo();
		}

		public void CreateBoard(int levelId)
		{
			TextAsset levelJson = Resources.Load<TextAsset> ($"Text/level{levelId.ToString()}");
			LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
			List<LevelInfo> levelInfo = levelData.data;

			_boardInfo.GetBoardSpecs(levelInfo, out int tilesCount);
			_boardTiles = new Tile.Tile[(int) BoardSize.x, (int) BoardSize.y, (int) BoardSize.z];
			_floors = new GameObject[(int) BoardSize.z];
			_middleTilePos = new Vector2((int) Math.Floor(BoardSize.x / 2), (int) Math.Floor(BoardSize.y / 2));

			_tileCreator = GetComponent<TileCreator>();
			_tileCreator.BoardSize = BoardSize;
			_tileCreator.BoardTiles = BoardTiles;
			_tileCreator.Floors = _floors;

			for (int i = 0; i < levelInfo.Count; i++)
			{
				int f = levelInfo[i].floor;
				List<LevelTiles> levelTiles = levelInfo[i].rows;
				for (int j = 0; j < levelTiles.Count; j++)
				{
					List<int> tiles = levelTiles[j].tiles;
					for (int k = 0; k < tiles.Count; k++)
					{
						bool isMiddleTile = IsMiddleTile(k, j, f);
						int state = tiles[k];
						switch (state)
						{
							// Handling the empty case. Should not create a tile.
							case (int) TileStates.Empty:
								_boardTiles[k, j, f] = null;
								break;
							// Handling the basic case
							case (int) TileStates.Single:
								_boardTiles[k, j, f] = _tileCreator.CreateTile(new Vector3(k, j, f), isMiddleTile);
								break;
							// Handling the case where the tile should be over 2 other tiles (in the middle horizontally)
							case (int) TileStates.DoubleH:
							{
								_boardTiles[k, j, f] = _tileCreator.CreateTile(new Vector3(k, j, f), isMiddleTile,
									TileStates.DoubleH);
								_boardTiles[k + 1, j, f] = _tileCreator.CreateTile(new Vector3(k + 1, j, f), isMiddleTile,
									TileStates.DummyH);
								SelectMiddleTile(IsMiddleTile(k + 1, j, f), k + 1, j, f);
								k++;
								break;
							}
							// Handling the case where the tile should be over 2 other tiles (in the middle vertically)
							case (int) TileStates.DoubleV:
							{
								Tile.Tile firstVTile = j - 1 >= 0 ? _boardTiles[k, j - 1, f] : null;
								if (!firstVTile || (firstVTile && firstVTile.State != TileStates.DoubleV))
								{
									_boardTiles[k, j, f] = _tileCreator.CreateTile(new Vector3(k, j, f), isMiddleTile,
										TileStates.DoubleV);
									_boardTiles[k, j + 1, f] = _tileCreator.CreateTile(new Vector3(k,  j + 1, f), isMiddleTile,
										TileStates.DummyV);
									SelectMiddleTile(IsMiddleTile(k, j + 1, f), k + 1, j, f);
								}
								break;
							}
						}

						SelectMiddleTile(isMiddleTile, k, j, f);
					}
				}
			}

			_boardImages = new BoardImages(tilesCount);
			_boardImages.AddImagesToTiles(_floors, levelData.fillMethod);

			GetComponent<BoardMatcher>().GetAvailableMoves();
		}

		public void RemoveMiddleTile()
		{
			if (!_middleTile.gameObject.activeSelf)
			{
				Vector3 index = _middleTile.Index;
				index.ToInts(out int x, out int y, out int f);
				_boardTiles[x, y, f] = null;
				Destroy(_middleTile.gameObject);
			}
		}

		private bool IsMiddleTile(int x, int y, int floor)
		{
			int mX = (int) _middleTilePos.x;
			int mY = (int) _middleTilePos.y;

			return mX == x && mY == y && floor == 0;
		}

		private void SelectMiddleTile(bool isMiddleTile, int k, int j, int f)
		{
			if (isMiddleTile)
			{
				// Used to center the board based on the middle tile.
				Tile.Tile middleTile = _boardTiles[k, j, f];
				if (!middleTile)
					_boardTiles[k, j, f] = _tileCreator.CreateTile(new Vector3(k, j, f),
						true, TileStates.Single, true);

				middleTile = _boardTiles[k, j, f];
				if (middleTile.State == TileStates.DummyH)
					_middleTile = _boardTiles[k - 1, j, f];
				else if (middleTile.State == TileStates.DummyV)
					_middleTile = _boardTiles[k, j - 1, f];
				else
					_middleTile = _boardTiles[k, j, f];
			}
		}

		public Tile.Tile[,,] BoardTiles => _boardTiles;
		public Vector3 BoardSize => _boardInfo.BoardSize;
		public Tile.Tile MiddleTile => _middleTile;
	}
}
