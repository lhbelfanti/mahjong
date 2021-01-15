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
					Tile tile = CreateTile(new Vector3(k, j, f), tiles, images);
					_boardTiles[i, j, f] = tile;
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
		int pairs = tilesCount / 2;

		List<Sprite> totalTiles = new List<Sprite>();

		Random rnd = new Random();
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

	private Tile CreateTile(Vector3 index, List<int> tiles, List<Sprite> images)
	{
		int state = tiles[(int) index.x];
		if (state == 0)
			return null;


		int x = (int) index.x;
		int y = (int) index.y;
		int z = (int) index.z;
		Rect tileRect = _tileGameObject.GetComponent<RectTransform>().rect;
		Vector3 boardPos = _boardGameObject.position;
		float xPos = boardPos.x + tileRect.width * x + _tileShift * x - _tileShift * z;
		float yPos = boardPos.y - tileRect.height * y + _tileShift * y;

		Tile tile = Instantiate(_tileGameObject, new Vector3(xPos, yPos, -z), Quaternion.identity);
		Color c = tile.Unselected;
		tile.SpriteRenderer.color = new Color(c.r, c.g, c.b, 1);
		if (!_floors[z])
		{
			_floors[z] = new GameObject("Floor " + z.ToString());
			_floors[z].transform.SetParent(_boardGameObject);
		}
		tile.transform.SetParent(_floors[z].transform);
		tile.transform.name = $"{x}x{y}x{z}";
		tile.SpriteRenderer.sprite = images[0];
		tile.TileId = images[0].name;
		images.RemoveAt(0);


		return tile;
	}
}
