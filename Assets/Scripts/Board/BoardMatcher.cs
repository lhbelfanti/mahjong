using System.Collections.Generic;
using UnityEngine;

public class BoardMatcher : MonoBehaviour
{
	private BoardCreator _boardCreator;
	private BoardSelector _boardSelector;

	private void Awake()
	{
		_boardCreator = GetComponent<BoardCreator>();
	}

	public bool CanBeSelected(Tile tile)
	{
		Vector3 index = tile.Index;

		GetNeighbours(index, out Tile top, out Tile right, out Tile left);

		if (!top)
			if (!right || !left || right.State == Tile.States.Dummy)
				return true;

		return false;
	}

	private void GetNeighbours(Vector3 index, out Tile top, out Tile right, out Tile left)
	{
		Tile[,,] boardTiles = _boardCreator.BoardTiles;
		Vector3 boardSize = _boardCreator.BoardSize;
		index.ToInts(out int x, out int y, out int floor);

		top = null;
		right = null;
		left = null;

		if (floor < (int) boardSize.z - 1)
			top = boardTiles[x, y, floor + 1];

		if (x > 0)
			left = boardTiles[x - 1, y, floor];

		if (x < (int) boardSize.x - 1)
			right = boardTiles[x + 1, y, floor];
	}

	public bool Match(List<Tile> selectedTiles)
	{
		string tileId = "";
		foreach (Tile t in selectedTiles)
		{
			if (tileId == "")
				tileId = t.Id;
			else if (tileId != t.Id)
				return false;
		}

		Tile[,,] boardTiles = _boardCreator.BoardTiles;
		foreach (Tile tile in selectedTiles)
		{
			tile.Index.ToInts(out int x, out int y, out int z);
			boardTiles[x, y, z] = null;
			if (tile.State == Tile.States.Double) // Removing the dummy tile
				boardTiles[x + 1, y, z] = null;
		}

		return true;
	}
}
