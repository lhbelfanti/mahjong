using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class BoardCreator : MonoBehaviour
{
	[SerializeField] private Tile _tileGameObject;
	[SerializeField] private Transform _boardGameObject;
	private List<List<Tile>> _boardTiles = new List<List<Tile>>();

	public void CreateBoard(int levelId)
	{
		TextAsset levelJson = Resources.Load<TextAsset> ("Text/level" + levelId.ToString());
		LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
		List<LevelInfo> levelInfo = levelData.data;

		List<Sprite> images = GetImages(levelInfo);

		for (int i = 0; i < levelInfo.Count; i++)
		{
			List<LevelTiles> levelTiles = levelInfo[i].rows;
			for (int j = 0; j < levelTiles.Count; j++)
			{
				_boardTiles.Add(new List<Tile>());
				List<int> tiles = levelTiles[j].tiles;
				for (int k = 0; k < tiles.Count; k++)
				{
					Tile tile = CreateTile(k, j, levelTiles, tiles, images);
					_boardTiles[j].Add(tile);
				}
			}
		}
	}

	private int CountTiles(List<LevelInfo> levelInfo)
	{
		int count = 0;
		for (int i = 0; i < levelInfo.Count; i++)
		{
			List<LevelTiles> levelTiles = levelInfo[i].rows;
			for (int j = 0; j < levelTiles.Count; j++)
			{
				List<int> tiles = levelTiles[j].tiles;
				for (int k = 0; k < tiles.Count; k++)
				{
					if (tiles[k] == 1)
						count++;
				}
			}
		}

		return count;
	}

	private List<Sprite> GetImages(List<LevelInfo> levelInfo)
	{
		Sprite[] images = Resources.LoadAll<Sprite>("Images/Tiles/");
		List<Sprite> sprites = images.ToList();
		int tilesCount = CountTiles(levelInfo);
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

	private Tile CreateTile(int x, int y, List<LevelTiles> levelTiles, List<int> tiles, List<Sprite> images)
	{
		int state = tiles[x];
		Vector3 localScale = _tileGameObject.transform.localScale;
		if (x == 0 && y == 0)
		{
			float bPosX = levelTiles.Count / 2 * localScale.x - localScale.x / 2;
			float bPosY = tiles.Count / 2 * localScale.y - localScale.y / 2;
			_boardGameObject.position = new Vector3(-bPosX, bPosY, 0);
		}

		Vector3 boardPos = _boardGameObject.position;
		float xPos = x * localScale.x + boardPos.x;
		float yPos = y * localScale.y - boardPos.y;
		Tile tile = Instantiate(_tileGameObject, new Vector3(xPos, -yPos, 0), Quaternion.identity);
		Color c = tile.Unselected;
		tile.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 0);
		tile.transform.SetParent(_boardGameObject);
		tile.transform.name = $"{x}x{y}";
		if (state != 0)
		{
			tile.GetComponent<SpriteRenderer>().sprite = images[0];
			tile.TileId = images[0].name;
			images.RemoveAt(0);
		}
		else
		{
			tile.gameObject.SetActive(false);
		}

		return tile;
	}
}
