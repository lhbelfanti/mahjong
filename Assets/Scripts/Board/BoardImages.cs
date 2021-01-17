using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class BoardImages
{
	public List<Sprite> GetImages(int tilesCount)
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
}
