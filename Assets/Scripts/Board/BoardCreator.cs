using System;
using System.Collections.Generic;
using Level;
using UnityEngine;
using Utils;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Board
{
	public class BoardCreator : MonoBehaviour
	{
		[SerializeField] private Tile.Tile tileGameObject;
		[SerializeField] private Transform boardGameObject;
		[SerializeField] private float tileShift;
		[SerializeField] private float zGap;

		private BoardInfo _boardInfo;
		private BoardImages _boardImages;
		private Tile.Tile[,,] _boardTiles;
		private GameObject[] _floors;
		private Vector2 _middleTilePos;
		private Tile.Tile _middleTile;

		private void Awake()
		{
			_boardInfo = new BoardInfo();
			_boardImages = new BoardImages();
		}

		public void CreateBoard(int levelId)
		{
			TextAsset levelJson = Resources.Load<TextAsset> ($"Text/level{levelId.ToString()}");
			LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
			List<LevelInfo> levelInfo = levelData.data;

			_boardInfo.GetBoardSpecs(levelInfo, out int tilesCount);
			List<Sprite> images = _boardImages.GetImages(tilesCount);

			_boardTiles = new Tile.Tile[(int) BoardSize.x, (int) BoardSize.y, (int) BoardSize.z];
			_floors = new GameObject[(int) BoardSize.z];
			_middleTilePos = new Vector2((int) Math.Floor(BoardSize.x / 2), (int) Math.Floor(BoardSize.y / 2));

			for (int i = 0; i < levelInfo.Count; i++)
			{
				int f = levelInfo[i].floor;
				List<LevelTiles> levelTiles = levelInfo[i].rows;
				for (int j = 0; j < levelTiles.Count; j++)
				{
					List<int> tiles = levelTiles[j].tiles;
					for (int k = 0; k < tiles.Count; k++)
					{
						int state = tiles[k];
						switch (state)
						{
							case (int) Tile.Tile.States.Empty: // Handling the empty case. Should not create a tile.
								_boardTiles[k, j, f] = null;
								break;
							case (int) Tile.Tile.States.Single: // Handling the basic case
								_boardTiles[k, j, f] = CreateTile(new Vector3(k, j, f), images);
								break;
							case (int) Tile.Tile.States.Double: // Handling the case where the tile should be over 2 other tiles (in the middle)
							{
								_boardTiles[k, j, f] = CreateTile(new Vector3(k, j, f), images, Tile.Tile.States.Double);
								_boardTiles[k + 1, j, f] = CreateDummyTile(new Vector3(k + 1, j, f));
								k++;
								break;
							}
						}

						if (IsMiddleTile(k, j, f))
						{
							Tile.Tile middleTile = _boardTiles[k, j, f];
							if (!middleTile)
								_boardTiles[k, j, f] = CreateTile(new Vector3(k, j, f), images, Tile.Tile.States.Single, true);

							if (middleTile.State == Tile.Tile.States.Dummy)
								_middleTile = _boardTiles[k - 1, j, f];
							else
								_middleTile = _boardTiles[k, j, f];
						}
					}
				}
			}

			GetComponent<BoardMatcher>().GetAvailableMoves();
		}

		private Tile.Tile CreateTile(Vector3 index, List<Sprite> images, Tile.Tile.States state = Tile.Tile.States.Single, bool fakeMiddle = false)
		{
			index.ToInts(out int x, out int y, out int floor);
			Rect tileRect = tileGameObject.GetComponent<RectTransform>().rect;
			Vector3 boardPos = boardGameObject.position;

			float xPos;
			if (state == Tile.Tile.States.Double)
			{
				bool shouldShift = ShouldShiftTile(index, (int) BoardSize.x, out Tile.Tile bottomTile);
				xPos = boardPos.x + tileRect.width * x + tileRect.width / 2 -
				       (shouldShift ? tileShift * (int) bottomTile.Index.z : 0);
			}
			else
				xPos = boardPos.x + tileRect.width * x - tileShift * floor;

			float yPos = boardPos.y - tileRect.height * y + tileShift * y;

			Tile.Tile tile = Instantiate(tileGameObject, new Vector3(xPos, yPos, -floor * zGap), Quaternion.identity);
			if (!_floors[floor])
			{
				_floors[floor] = new GameObject($"Floor {floor.ToString()}");
				_floors[floor].transform.SetParent(boardGameObject);
			}
			tile.transform.SetParent(_floors[floor].transform);
			tile.Id = images[0].name;
			tile.Index = index;
			tile.State = state;
			tile.transform.name = $"({x.ToString()}x{y.ToString()})-{floor.ToString()}-{tile.Id}";
			tile.SpriteRenderer.sprite = images[0];

			if (IsMiddleTile(x, y, floor))
				tile.transform.name += "--Middle";

			if (fakeMiddle)
				tile.gameObject.SetActive(false);
			else
				images.RemoveAt(0);

			return tile;
		}

		private Tile.Tile CreateDummyTile(Vector3 index)
		{
			index.ToInts(out int x, out int y, out int f);
			GameObject doubleExtra = new GameObject($"({x.ToString()}x{y.ToString()})-{f.ToString()}-Extra");
			doubleExtra.SetActive(false);
			Tile.Tile dummyTile = doubleExtra.AddComponent<Tile.Tile>();
			doubleExtra.AddComponent<Animator>();
			dummyTile.State = Tile.Tile.States.Dummy;
			dummyTile.transform.SetParent(_floors[f].transform);
			return dummyTile;
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

		private bool ShouldShiftTile(Vector3 index, int boardSize, out Tile.Tile bottomTile)
		{
			index.ToInts(out int x, out int y, out int floor);

			// If the tile is over another tile and the next one to the one that is at the bottom is null,
			// then I know that the tile should be positioned over the one that is in the bottom, shifted to the left in x.
			// Example -> My tile: (0, 0) floor 1 --- Bottom tile: (0, 0) floor 0 --- Next bottom tile: (1, 0) floor 0
			// If it's null then I know that in the level config there is a 2 in position (0,0,0) and position (1,0,0), and
			// I should position it over the bottom tile but shifted to the left in x.
			if (floor != 0 && _boardTiles[x, y, floor - 1])
			{
				if (x <= boardSize - 1 && _boardTiles[x + 1, y, floor - 1] &&
				    _boardTiles[x + 1, y, floor - 1].State == Tile.Tile.States.Dummy)
				{
					bottomTile = _boardTiles[x, y, floor - 1];
					return true;
				}
			}

			bottomTile = null;
			return false;
		}

		private bool IsMiddleTile(int x, int y, int floor)
		{
			int mX = (int) _middleTilePos.x;
			int mY = (int) _middleTilePos.y;

			return mX == x && mY == y && floor == 0;
		}

		public Tile.Tile[,,] BoardTiles => _boardTiles;
		public Vector3 BoardSize => _boardInfo.BoardSize;
		public Tile.Tile MiddleTile => _middleTile;
	}
}
