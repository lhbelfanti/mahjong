namespace LevelEditor
{
	public class DoubleVTileValidator : TileValidator
	{
		public DoubleVTileValidator(int[,,] tiles, EditorTile et) : base(tiles, et)
		{
		}

		public override bool Validate()
		{
			_initialMessage = $"Floor {_et.floor.ToString()} - {_et.x.ToString()}x{_et.y.ToString()} DoubleV Tile:\n";

			string msg1 = $"Error 1: There is already another tile in the position {_et.x.ToString()}x{_et.y.ToString()}.\n";

			string msg2 = $"Error 2: There is already another tile in the position {_et.x.ToString()}x{(_et.y + 1).ToString()}.\n";

			string msg3 = $"Error 3: Missing first tile underneath it (Floor {(_et.floor - 1).ToString()} - position {_et.x.ToString()}x{_et.y.ToString()})\n";

			string msg4 = $"Error 4: Missing second tile underneath it (Floor {(_et.floor - 1).ToString()} - position {(_et.x + 1).ToString()}x{_et.y.ToString()})\n";

			if (IsAnEmptyPosition(_et.x, _et.y, _et.floor, msg1) &&
			    IsAnEmptyPosition(_et.x + 1, _et.y, _et.floor, msg2) &&
			    ValidateUnderneathTile(msg3, msg4))
				return true;

			LogError();
			return false;
		}


		private bool ValidateUnderneathTile(string msg1, string msg2)
		{
			if (_et.floor != 0)
			{
				bool validation1 = IsThereATile(_et.x, _et.y, _et.floor - 1, msg1);
				bool validation2 = IsThereATile(_et.x + 1, _et.y, _et.floor - 1, msg2);

				return validation1 && validation2;
			}

			return true;
		}
	}
}
