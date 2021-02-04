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
		}

		public List<EditorTile> Tiles()
		{
			return _tiles.OrderBy(o => o.floor).ThenBy(o => o.x).ToList();
		}
	}
}
