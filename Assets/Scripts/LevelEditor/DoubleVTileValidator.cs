namespace LevelEditor
{
	public class DoubleVTileValidator : TileValidator
	{
		public DoubleVTileValidator(int[,,] tiles, EditorTile et) : base(tiles, et)
		{
		}

		public override bool Validate()
		{
			_initialMessage = $"Floor {_et.floor.ToString()} - {_et.y.ToString()}x{_et.x.ToString()} DoubleH Tile:\n";

			string msg1 = $"Error 1: There is already another tile in the position {_et.y.ToString()}x{_et.x.ToString()}.\n";

			string msg2 = $"Error 2: There is already another tile in the position {_et.y.ToString()}x{(_et.x + 1).ToString()}.\n";

			string msg3 = $"Error 3: Missing first tile underneath it (Floor {(_et.floor - 1).ToString()} - position {_et.y.ToString()}x{_et.x.ToString()})\n";

			string msg4 = $"Error 4: Missing second tile underneath it (Floor {(_et.floor - 1).ToString()} - position {(_et.y + 1).ToString()}x{_et.x.ToString()})\n";

			if (IsAnEmptyPosition(_et.x, _et.y, _et.floor, msg1) &&
			    IsAnEmptyPosition(_et.x, _et.y + 1, _et.floor, msg2) &&
			    ValidateUnderneathTile(msg3, msg4))
				return true;

			LogError();
			return false;
		}


		private bool ValidateUnderneathTile(string msg1, string msg2)
		{
			if (_et.floor != 0)
			{
				return IsThereATile(_et.x, _et.y, _et.floor - 1, msg1) &&
				       IsThereATile(_et.x, _et.y + 1, _et.floor - 1, msg2);
			}

			return true;
		}
	}
}
