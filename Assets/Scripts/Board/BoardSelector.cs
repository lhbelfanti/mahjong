using System.Collections.Generic;
using UnityEngine;

public class BoardSelector : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] private int _matchNumber;

	private List<Tile> _selectedTiles = new List<Tile>();
	private BoardMatcher _boardMatcher;

	private void Awake()
	{
		_boardMatcher = GetComponent<BoardMatcher>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.up);
			if (hit.collider)
			{
				Tile tile = hit.collider.transform.parent.GetComponent<Tile>();
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

	private void SelectTile(Tile tile)
	{
		if (_selectedTiles.Count == _matchNumber && !_selectedTiles.Contains(tile))
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

		if (_selectedTiles.Count == _matchNumber)
			MatchTiles();
	}

	private void MatchTiles()
	{
		if (_boardMatcher.Match(_selectedTiles))
		{
			foreach (Tile t in _selectedTiles)
			{
				t.MatchAnim();
			}
		}
		else
			UnselectTiles();
	}

	private void UnselectTiles()
	{
		foreach (Tile t in _selectedTiles)
		{
			t.WrongMatchAnim();
			t.Selected(false);
		}
		_selectedTiles.Clear();
	}
}
