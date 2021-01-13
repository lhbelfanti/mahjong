using System.Collections.Generic;
using UnityEngine;

public class BoardSelector : MonoBehaviour
{

	[SerializeField] private Camera _camera;
	[SerializeField] private int _matchNumber;

	private List<Tile> _selectedTiles = new List<Tile>();

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.up);
			if (hit.collider)
			{
				Tile tile = hit.collider.GetComponent<Tile>();
				SelectTile(tile);
			}
			else
			{
				UnselectAllTiles();
			}
		}
	}

	private void SelectTile(Tile tile)
	{
		if (_selectedTiles.Count == _matchNumber && !_selectedTiles.Contains(tile))
			UnselectAllTiles();

		if (!_selectedTiles.Contains(tile))
		{
			_selectedTiles.Add(tile);
			tile.GetComponent<SpriteRenderer>().color = tile.Selected;
		}
		else
		{
			_selectedTiles.Remove(tile);
			tile.GetComponent<SpriteRenderer>().color = tile.Unselected;
		}

		if (_selectedTiles.Count == _matchNumber)
			MatchTiles();
	}

	private void MatchTiles()
	{
		if (AreAllSame())
		{
			foreach (Tile t in _selectedTiles)
			{
				t.MatchAnim();

			}
		}
		else
		{
			foreach (Tile t in _selectedTiles)
			{
				t.WrongMatchAnim();

			}
			UnselectAllTiles();
		}
	}

	private bool AreAllSame()
	{
		string tileId = "";
		foreach (Tile t in _selectedTiles)
		{
			if (tileId == "")
				tileId = t.TileId;
			else if (tileId != t.TileId)
				return false;
		}

		return true;
	}

	private void UnselectAllTiles()
	{
		foreach (Tile t in _selectedTiles)
		{
			t.GetComponent<SpriteRenderer>().color = t.Unselected;
		}
		_selectedTiles.Clear();
	}
}
