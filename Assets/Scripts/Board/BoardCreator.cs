using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class BoardCreator : MonoBehaviour
{
	[SerializeField] private Tile _tileGameObject;
	[SerializeField] private Transform _boardGameObject;
	[SerializeField] private float _tileShift;
	private Tile[,,] _boardTiles;
	private GameObject[] _floors;

	public void CreateBoard(int levelId)
	{
		TextAsset levelJson = Resources.Load<TextAsset> ("Text/level" + levelId.ToString());
		LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
		List<LevelInfo> levelInfo = levelData.data;

		GetBoardSpecs(levelInfo, out int tilesCount, out Vector3 boardSize);
		List<Sprite> images = GetImages(tilesCount);

		_boardTiles = new Tile[(int) boardSize.x, (int) boardSize.y, (int) boardSize.z];
		_floors = new GameObject[(int) boardSize.z];


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
						case 0: // Handling the empty case. Should not create a tile.
							_boardTiles[k, j, f] = null;
							break;
						case 1: // Handling the basic case
							_boardTiles[k, j, f] = CreateTile(new Vector3(k, j, f), images);
							break;
						case 2: // Handling the case where the tile should be over 2 other tiles (in the middle)
						{
							_boardTiles[k, j, f] = CreateTile(new Vector3(k, j, f), images, 2, (int) boardSize.x);
							_boardTiles[k + 1, j, f] = null;
							k++;
							break;
						}
					}
				}
			}
		}
	}

	private void GetBoardSpecs(List<LevelInfo> levelInfo, out int tilesCount, out Vector3 boardSize)
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
					if (tiles[k] != 0)
						tc++;
				}

				if (rows < tiles.Count)
					rows = tiles.Count;
			}

			if (cols < levelTiles.Count)
				cols = levelTiles.Count;

			floors++;
		}

		tilesCount = tc;
		boardSize = new Vector3(rows, cols, floors);
	}

	private List<Sprite> GetImages(int tilesCount)
	{
		Sprite[] images = Resources.LoadAll<Sprite>("Images/Tiles/");
		List<Sprite> sprites = images.ToList();
		Random rnd = new Random();

		int pairs = tilesCount / 2;
		while (pairs > sprites.Count)
		{
			int pos = rnd.Next(0, sprites.Count - 1);
			sprites.Add(sprites[pos]);
		}

		List<Sprite> totalTiles = new List<Sprite>();

		for (int i = 0; i < pairs; i++)
		{
			int pos = rnd.Next(0, sprites.Count - 1);
			totalTiles.Add(sprites[pos]);
			totalTiles.Add(sprites[pos]);
			sprites.RemoveAt(pos);
		}
		totalTiles.Shuffle();
		return totalTiles;
	}

	private Tile CreateTile(Vector3 index, List<Sprite> images, int state = 1, int boardSize = 0)
	{
		int x = (int) index.x;
		int y = (int) index.y;
		int floor = (int) index.z;
		Rect tileRect = _tileGameObject.GetComponent<RectTransform>().rect;
		Vector3 boardPos = _boardGameObject.position;

		float xPos;
		if (state == 2)
		{
			bool shouldShift = ShouldShiftTile(index, boardSize, out Tile bottomTile);
			xPos = boardPos.x + tileRect.width * x + tileRect.width / 2 -
			       (shouldShift ? _tileShift * (int) bottomTile.TileIndex.z : 0);
		}
		else
			xPos = boardPos.x + tileRect.width * x - _tileShift * floor;

		float yPos = boardPos.y - tileRect.height * y + _tileShift * y;

		Tile tile = Instantiate(_tileGameObject, new Vector3(xPos, yPos, -floor), Quaternion.identity);
		Color c = tile.Unselected;
		tile.SpriteRenderer.color = new Color(c.r, c.g, c.b, 1);
		if (!_floors[floor])
		{
			_floors[floor] = new GameObject("Floor " + floor.ToString());
			_floors[floor].transform.SetParent(_boardGameObject);
		}
		tile.transform.SetParent(_floors[floor].transform);
		tile.TileId = images[0].name;
		tile.TileIndex = index;
		tile.transform.name = $"({x.ToString()}x{y.ToString()})-{floor.ToString()}-{tile.TileId}";
		tile.SpriteRenderer.sprite = images[0];
		images.RemoveAt(0);

		return tile;
	}

	private bool ShouldShiftTile(Vector3 index, int boardSize, out Tile bottomTile)
	{
		int x = (int) index.x;
		int y = (int) index.y;
		int floor = (int) index.z;

		// If the tile is over another tile and the next one to the one that is at the bottom is null,
		// then I know that the tile should position over the one that is in the bottom, shifted to the left in x.
		// Example -> My tile: (0, 0) floor 1 --- Bottom tile: (0, 0) floor 0 --- Next bottom tile: (1, 0) floor 0
		// If it's null then I know that in the level config there is a 2 in position (0,0,0) and position (1,0,0), and
		// I should position it over the bottom tile but shifted to the left in x.
		if (floor != 0 && _boardTiles[x, y, floor - 1])
		{
			if (x <= boardSize - 1 && !_boardTiles[x + 1, y, floor - 1])
			{
				bottomTile = _boardTiles[x, y, floor - 1];
				return true;
			}
		}

		bottomTile = null;
		return false;
	}

	public Tile[,,] BoardTiles => _boardTiles;
}
