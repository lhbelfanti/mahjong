using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Random = System.Random;
using TileStates = Board.Tile.TileCreator.TileStates;

namespace Board
{
	public class BoardImages
	{
		private enum FillMethod
		{
			Random = 0,
			ByFloor = 1
		}

		private List<Sprite> _tileSprites;

		public BoardImages(int tilesCount)
		{
			_tileSprites = new List<Sprite>();
			LoadImages(tilesCount);
		}

		private void LoadImages(int tilesCount)
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

			for (int i = 0; i < pairs; i++)
			{
				int pos = rnd.Next(0, sprites.Count - 1);
				_tileSprites.Add(sprites[pos]);
				_tileSprites.Add(sprites[pos]);
				sprites.RemoveAt(pos);
			}
		}

		public void AddImagesToTiles(GameObject[] floors, int fillMethod)
		{
			switch (fillMethod)
			{
				case (int) FillMethod.Random:
					FillAllRandom(floors);
					break;
				case (int) FillMethod.ByFloor:
					FillByFloor(floors);
					break;
			}
		}


		private void FillAllRandom(GameObject[] floors)
		{
			List<Tile.Tile> tiles = new List<Tile.Tile>();
			foreach (GameObject f in floors)
			{
				List<GameObject> children = f.GetActiveChildren();
				foreach (GameObject c in children)
				{

					Tile.Tile tile = c.GetComponent<Tile.Tile>();
					if (tile.State == TileStates.Single ||
					    tile.State == TileStates.DoubleH ||
					    tile.State == TileStates.DoubleV)
					{
						tiles.Add(tile);
					}
				}
			}

			FillData(tiles);
		}

		private void FillByFloor(GameObject[] floors)
		{
			foreach (GameObject f in floors)
			{
				List<Tile.Tile> tiles = new List<Tile.Tile>();
				List<GameObject> children = f.GetActiveChildren();
				foreach (GameObject c in children)
				{
					Tile.Tile tile = c.GetComponent<Tile.Tile>();
					if (tile.State == TileStates.Single ||
					    tile.State == TileStates.DoubleH ||
					    tile.State == TileStates.DoubleV)
					{
						tiles.Add(tile);
					}
				}

				FillData(tiles);
			}
		}

		private void FillData(List<Tile.Tile> tiles)
		{
			tiles.Shuffle();

			foreach (Tile.Tile tile in tiles)
			{
				tile.Id = _tileSprites[0].name;
				tile.transform.name += $"-{tile.Id}";
				tile.SetTexture(_tileSprites[0].texture);
				_tileSprites.RemoveAt(0);
			}

		}
	}
}
