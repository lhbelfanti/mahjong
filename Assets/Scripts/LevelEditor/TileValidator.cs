using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class TileValidator : ITileValidator
	{
		protected readonly EditorTile _et;
		protected string _initialMessage;

		private int[,,] _tiles;
		private  string _message;

		public TileValidator(int[,,] tiles, EditorTile et)
		{
			_tiles = tiles;
			_et = et;
		}

		public virtual bool Validate()
		{
			throw new System.NotImplementedException();
		}

		protected void LogError()
		{
			Debug.LogError(_initialMessage + _message);
		}

		protected bool IsAnEmptyPosition(int x, int y, int f, string msg)
		{
			int tileType = _tiles[x, y, f];
			if (tileType != (int) TileCreator.TileTypes.Empty &&
			    tileType != (int) TileCreator.TileTypes.DummyH &&
			    tileType != (int) TileCreator.TileTypes.DummyV)
			{
				_message += msg;
				return false;
			}

			return true;
		}

		protected bool IsThereATile(int x, int y, int f, string msg)
		{
			if (_tiles[x, y, f] == (int) TileCreator.TileTypes.Empty)
			{
				_message += msg;
				return false;
			}

			return true;
		}
	}
}
