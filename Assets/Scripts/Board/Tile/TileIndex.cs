using UnityEngine;

namespace Board.Tile
{
	public struct TileIndex
	{
		public int x;
		public int y;
		public int floor;

		public TileIndex(float ix, float iy, float iz)
		{
			x = (int) ix;
			y = (int) iy;
			floor = (int) iz;
		}
	}
}
