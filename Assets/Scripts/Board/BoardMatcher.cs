using System.Collections.Generic;
using Game;
using UnityEngine;
using Utils;

namespace Board
{
	public class BoardMatcher : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;

		private BoardCreator _boardCreator;
		private BoardSelector _boardSelector;

		private void Awake()
		{
			_boardCreator = GetComponent<BoardCreator>();
		}

		public bool CanBeSelected(Tile.Tile tile)
		{
			Vector3 index = tile.Index;

			GetNeighbours(index, out Tile.Tile top, out Tile.Tile right, out Tile.Tile left);

			if (!top)
				if (!right || !left || right.State == Tile.Tile.States.Dummy)
					return true;

			return false;
		}

		private void GetNeighbours(Vector3 index, out Tile.Tile top, out Tile.Tile right, out Tile.Tile left)
		{
			Tile.Tile[,,] boardTiles = _boardCreator.BoardTiles;
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

		public bool Match(List<Tile.Tile> selectedTiles)
		{
			string tileId = "";
			foreach (Tile.Tile t in selectedTiles)
			{
				if (tileId == "")
					tileId = t.Id;
				else if (tileId != t.Id)
					return false;
			}

			Tile.Tile[,,] boardTiles = _boardCreator.BoardTiles;
			foreach (Tile.Tile tile in selectedTiles)
			{
				tile.Index.ToInts(out int x, out int y, out int z);
				boardTiles[x, y, z] = null;
				if (tile.State == Tile.Tile.States.Double) // Removing the dummy tile
					boardTiles[x + 1, y, z] = null;
			}

			if (LevelWon())
				gameManager.SwitchGameState(GameManager.GameState.Win);
			else if (LevelLose())
				gameManager.SwitchGameState(GameManager.GameState.Lose);

			return true;
		}

		private bool LevelLose()
		{
			List<Tile.Tile> availableMoves = new List<Tile.Tile>();
			Tile.Tile[,,] boardTiles = _boardCreator.BoardTiles;
			foreach (Tile.Tile tile in boardTiles)
			{
				if (tile && tile.State != Tile.Tile.States.Dummy && CanBeSelected(tile))
					availableMoves.Add(tile);
			}

			for (int i = 0; i < availableMoves.Count; i++)
			{
				string currentTile = availableMoves[i].Id;
				for (int j = i + 1; j < availableMoves.Count; j++)
				{
					if (currentTile == availableMoves[j].Id)
						return false;
				}
			}

			return true;
		}

		private bool LevelWon()
		{
			Tile.Tile[,,] boardTiles = _boardCreator.BoardTiles;
			foreach (Tile.Tile tile in boardTiles)
			{
				if (tile)
					return false;
			}

			return true;
		}
	}
}
