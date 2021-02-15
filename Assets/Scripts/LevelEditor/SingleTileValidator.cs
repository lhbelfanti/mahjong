namespace LevelEditor
{
	public class SingleTileValidator : TileValidator
	{
		public SingleTileValidator(int[,,] tiles, EditorTile et) : base(tiles, et)
		{
		}
		public override bool Validate()
		{
			_initialMessage = $"Floor {_et.floor.ToString()} - {_et.x.ToString()}x{_et.y.ToString()} Single Tile:\n";

			string msg1 = $"Error 1: There is already another tile in the position {_et.x.ToString()}x{_et.y.ToString()}.\n";

			string msg2 = $"Error 2: Missing tile underneath it (in Floor {(_et.floor - 1).ToString()} - position {_et.x.ToString()}x{_et.y.ToString()}).\n";

			if (IsAnEmptyPosition(_et.x, _et.y, _et.floor, msg1) && ValidateUnderneathTile(msg2))
				return true;

			LogError();
			return false;
		}

		private bool ValidateUnderneathTile(string msg)
		{
			if (_et.floor != 0)
				return IsThereATile(_et.x, _et.y, _et.floor - 1, msg);

			return true;
		}
	}
}
