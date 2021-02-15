using UnityEngine;
using Utils;
using Vector3 = UnityEngine.Vector3;

namespace Board.Tile
{
	public class TileCreator : MonoBehaviour
	{
		[SerializeField] private Tile tileGameObject;
		[SerializeField] private Transform boardGameObject;
		[SerializeField] private float tileShift;
		[SerializeField] private float zGap;

		public enum TileTypes
		{
			Empty = 0,
			Single = 1,
			DoubleH = 2,
			DummyH = 3,
			DoubleV = 4,
			DummyV = 5
		}

		public Tile CreateTile(Vector3 index, bool isMiddleTile, TileTypes type = TileTypes.Single, bool fakeMiddle = false)
		{
			index.ToInts(out int x, out int y, out int floor);
			Vector2 pos = GetTilePosition(index, type);

			Tile tile = Instantiate(tileGameObject, new Vector3(pos.x, pos.y, -floor * zGap), Quaternion.identity);
			if (!Floors[floor])
			{
				Floors[floor] = new GameObject($"Floor {floor.ToString()}");
				Floors[floor].transform.SetParent(boardGameObject);
			}
			tile.transform.SetParent(Floors[floor].transform);
			tile.Index = index;
			tile.Type = type;
			tile.transform.name = $"({x.ToString()}x{y.ToString()})-{floor.ToString()}";

			if (type == TileTypes.DummyH || type == TileTypes.DummyV)
			{
				tile.transform.name += "-Extra";
				tile.gameObject.SetActive(false);
			}

			if (isMiddleTile)
				tile.transform.name += "-Middle";

			if (fakeMiddle)
				tile.gameObject.SetActive(false);



			return tile;
		}

		private Vector2 GetTilePosition(Vector3 index, TileTypes type = TileTypes.Single)
		{
			index.ToInts(out int x, out int y, out int floor);
			Rect tileRect = tileGameObject.GetComponent<RectTransform>().rect;
			Vector3 boardPos = boardGameObject.position;

			float xPos;
			float yPos;
			switch (type)
			{
				case TileTypes.DoubleH:
					bool shouldShiftX = ShouldShiftTileX(index, out Tile bottomTile);
					xPos = boardPos.x + tileRect.width * x + tileRect.width / 2 -
					       (shouldShiftX ? tileShift * (int) bottomTile.Index.z : 0);
					bool shouldShiftY = ShouldShiftTileY(index);
					yPos = boardPos.y - tileRect.height * y + tileShift * y -
					       (shouldShiftY ? tileRect.height / 2 : 0);
					break;
				case TileTypes.DoubleV:
					xPos = boardPos.x + tileRect.width * x - tileShift * floor;
					yPos = boardPos.y - tileRect.height * y - tileRect.height / 2 + tileShift * y;
					break;
				default:
					xPos = boardPos.x + tileRect.width * x - tileShift * floor;
					yPos = boardPos.y - tileRect.height * y + tileShift * y;
					break;
			}

			return new Vector2(xPos, yPos);
		}


		private bool ShouldShiftTileX(Vector3 index, out Tile bottomTile)
		{
			index.ToInts(out int x, out int y, out int floor);
			int boardSize = (int) BoardSize.x;

			/* If the tile is over another tile and the next one to the one that is at the bottom is null,
			 then I know that the tile should be positioned over the one that is in the bottom, shifted to the left in x.
			 Example -> My tile: (0, 0) floor 1 --- Bottom tile: (0, 0) floor 0 --- Next bottom tile: (1, 0) floor 0
			 If it's null then I know that in the level config there is a 2 in position (0,0,0) and position (1,0,0), and
			 I should position it over the bottom tile but shifted to the left in x. */
			if (floor != 0 && BoardTiles[x, y, floor - 1])
			{
				Tile dummyH = BoardTiles[x + 1, y, floor - 1];
				if (x <= boardSize - 1 && dummyH && dummyH.Type == TileTypes.DummyH)
				{
					bottomTile = BoardTiles[x, y, floor - 1];
					return true;
				}
			}

			bottomTile = null;
			return false;
		}

		private bool ShouldShiftTileY(Vector3 index)
		{
			index.ToInts(out int x, out int y, out int floor);

			if (floor != 0 && BoardTiles[x, y, floor - 1] && BoardTiles[x, y, floor - 1].Type == TileTypes.DoubleV)
				return true;

			return false;
		}

		public Vector3 BoardSize { get; set; }
		public Tile[,,] BoardTiles { get; set; }
		public GameObject[] Floors { get; set; }
	}
}
