using System.Collections.Generic;
using UnityEngine;

namespace Board
{
	public class BoardSelector : MonoBehaviour
	{
		[SerializeField] private Camera mainCamera;
		[SerializeField] private int matchNumber;

		private List<Tile.Tile> _selectedTiles;
		private BoardMatcher _boardMatcher;

		private void Awake()
		{
			_selectedTiles = new List<Tile.Tile>();
			_boardMatcher = GetComponent<BoardMatcher>();
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(ray, out RaycastHit hit))
				{
					Tile.Tile tile = hit.collider.transform.parent.GetComponent<Tile.Tile>();
					if (_boardMatcher.CanBeSelected(tile))
						SelectTile(tile);
					else
					{
						// If the tile cannot be selected, the wrong match animation should be played
						_selectedTiles.Add(tile);
						UnselectTiles();
					}
				}
			}


		}

		private void SelectTile(Tile.Tile tile)
		{
			if (_selectedTiles.Count == matchNumber && !_selectedTiles.Contains(tile))
				UnselectTiles();

			if (!_selectedTiles.Contains(tile))
			{
				_selectedTiles.Add(tile);
				tile.Selected(true);
			}
			else
			{
				_selectedTiles.Remove(tile);
				tile.Selected(false);
			}

			if (_selectedTiles.Count == matchNumber)
				MatchTiles();
		}

		private void MatchTiles()
		{
			if (_boardMatcher.Match(_selectedTiles))
			{
				foreach (Tile.Tile t in _selectedTiles)
				{
					t.MatchAnim();
				}
			}
			else
				UnselectTiles();
		}

		private void UnselectTiles()
		{
			foreach (Tile.Tile t in _selectedTiles)
			{
				t.WrongMatchAnim();
				t.Selected(false);
			}
			_selectedTiles.Clear();
		}
	}
}
