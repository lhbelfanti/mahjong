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
			if (_tiles[x, y, f] != 0)
			{
				_message += msg;
				return false;
			}

			return true;
		}

		protected bool IsThereATile(int x, int y, int f, string msg)
		{
			if (_tiles[x, y, f] == 0)
			{
				_message += msg;
				return false;
			}

			return true;
		}
	}
}
