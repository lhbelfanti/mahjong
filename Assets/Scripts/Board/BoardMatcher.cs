using System.Collections.Generic;
using Board.Tile;
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
		private List<Tile.Tile> _availableMoves;

		private void Awake()
		{
			_boardCreator = GetComponent<BoardCreator>();
		}

		public bool CanBeSelected(Tile.Tile tile)
		{
			Vector3 index = tile.Index;

			GetNeighbours(index, out Tile.Tile top, out Tile.Tile right, out Tile.Tile left);

			if (!top && (!right || !left || right.Type == TileCreator.TileTypes.DummyH))
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
				if (tile.Type == TileCreator.TileTypes.DoubleH) // Removing the dummy tile
					boardTiles[x + 1, y, z] = null;
				else if (tile.Type == TileCreator.TileTypes.DoubleV) // Removing the dummy tile
					boardTiles[x, y + 1, z] = null;
			}

			if (LevelWon())
				gameManager.SwitchGameState(GameManager.GameState.Win);
			else if (LevelLose())
				gameManager.SwitchGameState(GameManager.GameState.Lose);

			return true;
		}

		private bool LevelLose()
		{
			GetAvailableMoves();
			for (int i = 0; i < _availableMoves.Count; i++)
			{
				string currentTile = _availableMoves[i].Id;
				for (int j = i + 1; j < _availableMoves.Count; j++)
				{
					if (currentTile == _availableMoves[j].Id)
						return false;
				}
			}

			return true;
		}

		public void GetAvailableMoves()
		{
			_availableMoves = new List<Tile.Tile>();
			Tile.Tile[,,] boardTiles = _boardCreator.BoardTiles;
			foreach (Tile.Tile tile in boardTiles)
			{
				if (tile && tile.Type != TileCreator.TileTypes.DummyH && CanBeSelected(tile))
					_availableMoves.Add(tile);
			}
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

		public List<Tile.Tile> GetPossibleMatch()
		{
			List<Tile.Tile> match = new List<Tile.Tile>();

			for (int i = 0; i < _availableMoves.Count; i++)
			{
				string currentTile = _availableMoves[i].Id;
				for (int j = i + 1; j < _availableMoves.Count; j++)
				{
					if (currentTile == _availableMoves[j].Id)
					{
						match.Add(_availableMoves[i]);
						match.Add(_availableMoves[j]);
						return match;
					}
				}
			}

			return match;
		}
	}
}
