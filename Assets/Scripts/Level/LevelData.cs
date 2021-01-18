using System;
using System.Collections.Generic;

namespace Level
{
	[Serializable]
	public class LevelData
	{
		public int level;
		public int fillMethod;
		public List<LevelInfo> data;
	}

	[Serializable]
	public class LevelInfo
	{
		public int floor;
		public List<LevelTiles> rows;
	}

	[Serializable]
	public class LevelTiles
	{
		public List<int> tiles;
	}
}
