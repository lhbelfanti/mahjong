using System;
using System.Collections.Generic;
using UnityEngine;

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
		for (int i = 0; i < levelInfo.Count; i++)
		{
			List<LevelTiles> levelTiles = levelInfo[i].rows;
			for (int j = 0; j < levelTiles.Count; j++)
			{
				boardTiles.Add(new List<Tile>());
				List<int> tiles = levelTiles[j].tiles;
				for (int k = 0; k < tiles.Count; k++)
				{
					var localScale = tileGameObject.transform.localScale;
					if (j == 0 && k == 0)
					{
						float bPosX = ((levelTiles.Count / 2) * localScale.x ) - localScale.x / 2;
						float bPosY = ((tiles.Count / 2) * localScale.y) - localScale.y / 2;
						boardGameObject.position = new Vector3(-bPosX, bPosY, 0);
					}
					float xPos = (k * localScale.x) + boardGameObject.position.x;
					float yPos = (j * localScale.y) - boardGameObject.position.y;
					Tile tileGO = Instantiate(tileGameObject, new Vector3(xPos, yPos, 0), Quaternion.identity);
					tileGO.transform.parent = boardGameObject;
					tileGO.transform.name = $"{j}x{k}";
					tileGO.CurrentState = tiles[k];
					boardTiles[j].Add(tileGO);
				}
			}
		}
	}
}
