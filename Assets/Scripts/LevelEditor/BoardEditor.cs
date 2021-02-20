using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelEditor
{
	public class BoardEditor : MonoBehaviour
	{
		private List<EditorTile> _tiles = new List<EditorTile>();

		public void AddTile(EditorTile tile)
		{
			if (!_tiles.Contains(tile))
				_tiles.Add(tile);
		}

		public void RemoveTile(EditorTile tile)
		{
			_tiles.Remove(tile);
			DestroyImmediate(tile.gameObject);
		}

		public void RemoveTilesInFloor(int floor)
		{
			for (int i = _tiles.Count - 1; i > -1; i--)
			{
				if (_tiles[i].floor == floor)
					_tiles.RemoveAt(i);
			}

			for (int j = 0; j < _tiles.Count; j++)
			{
				if (_tiles[j].floor > floor)
					_tiles[j].floor--;
			}
		}

		public void RemoveAllTiles()
		{
			_tiles = new List<EditorTile>();
		}

		public List<EditorTile> Tiles()
		{
			return _tiles.Count > 0 ? _tiles.OrderBy(o => o.floor).ThenBy(o => o.x).ToList() : _tiles;
		}
	}
}
