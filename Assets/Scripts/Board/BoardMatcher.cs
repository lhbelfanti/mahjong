using System.Collections.Generic;
using Board.Tile;
using Game;
using UnityEngine;

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
			TileIndex index = tile.Index;

			GetNeighbours(index, out Tile.Tile top, out Tile.Tile right, out Tile.Tile left);

			if (!top && (!right || !left || right.Type == TileCreator.TileTypes.DummyH))
				return true;

			return false;
		}

		private void GetNeighbours(TileIndex ti, out Tile.Tile top, out Tile.Tile right, out Tile.Tile left)
		{
			Tile.Tile[,,] boardTiles = _boardCreator.BoardTiles;
			Vector3 boardSize = _boardCreator.BoardSize;

			top = null;
			right = null;
			left = null;

			if (ti.floor < (int) boardSize.z - 1)
				top = boardTiles[ti.x, ti.y, ti.floor + 1];

			if (ti.x > 0)
				left = boardTiles[ti.x - 1, ti.y, ti.floor];

			if (ti.x < (int) boardSize.x - 1)
				right = boardTiles[ti.x + 1, ti.y, ti.floor];
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
				TileIndex ti = tile.Index;
				boardTiles[ti.x, ti.y, ti.floor] = null;
				if (tile.Type == TileCreator.TileTypes.DoubleH) // Removing the dummy tile
					boardTiles[ti.x + 1, ti.y, ti.floor] = null;
				else if (tile.Type == TileCreator.TileTypes.DoubleV) // Removing the dummy tile
					boardTiles[ti.x, ti.y + 1, ti.floor] = null;
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
