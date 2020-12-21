using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BoardManager : MonoBehaviour
{
	[SerializeField]
	private Tile tileGameObject;
	[SerializeField]
	private Transform boardGameObject;

	private List<List<Tile>> boardTiles = new List<List<Tile>>();

	void Start()
	{
		CreateBoard("1");
	}

	private void CreateBoard(string levelId)
	{
		TextAsset levelJson = Resources.Load<TextAsset> ("Text/level" + levelId);
		LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
		List<LevelInfo> levelInfo = levelData.data;

		List<Sprite> images = GetImages(levelInfo);

		for (int i = 0; i < levelInfo.Count; i++)
		{
			List<LevelTiles> levelTiles = levelInfo[i].rows;
			for (int j = 0; j < levelTiles.Count; j++)
			{
				boardTiles.Add(new List<Tile>());
				List<int> tiles = levelTiles[j].tiles;
				for (int k = 0; k < tiles.Count; k++)
				{
					Tile tile = CreateTile(k, j, levelTiles, tiles, images);
					boardTiles[j].Add(tile);
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
		Vector3 localScale = tileGameObject.transform.localScale;
		if (x == 0 && y == 0)
		{
			float bPosX = levelTiles.Count / 2 * localScale.x - localScale.x / 2;
			float bPosY = tiles.Count / 2 * localScale.y - localScale.y / 2;
			boardGameObject.position = new Vector3(-bPosX, bPosY, 0);
		}

		Vector3 boardPos = boardGameObject.position;
		float xPos = x * localScale.x + boardPos.x;
		float yPos = y * localScale.y - boardPos.y;
		Tile tileGO = Instantiate(tileGameObject, new Vector3(xPos, -yPos, 0), Quaternion.identity);
		tileGO.transform.SetParent(boardGameObject);
		tileGO.transform.name = $"{x}x{y}";
		if (state != 0)
		{
			tileGO.GetComponent<SpriteRenderer>().sprite = images[0];
			images.RemoveAt(0);
		}
		tileGO.CurrentState = state;


		return tileGO;
	}
}
