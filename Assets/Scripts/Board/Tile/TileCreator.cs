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

		public Tile CreateTile(TileIndex ti, bool isMiddleTile, TileTypes type = TileTypes.Single, bool fakeMiddle = false)
		{
			Vector2 pos = GetTilePosition(ti, type);

			Tile tile = Instantiate(tileGameObject, new Vector3(pos.x, pos.y, -ti.floor * zGap), Quaternion.identity);
			if (!Floors[ti.floor])
			{
				Floors[ti.floor] = new GameObject($"Floor {ti.floor.ToString()}");
				Floors[ti.floor].transform.SetParent(boardGameObject);
			}
			tile.transform.SetParent(Floors[ti.floor].transform);
			tile.Index = ti;
			tile.Type = type;
			tile.transform.name = $"({ti.x.ToString()}x{ti.y.ToString()})-{ti.floor.ToString()}";

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

		private Vector2 GetTilePosition(TileIndex ti, TileTypes type = TileTypes.Single)
		{
			Rect tileRect = tileGameObject.GetComponent<RectTransform>().rect;
			Vector3 boardPos = boardGameObject.position;

			float xPos;
			float yPos;
			switch (type)
			{
				case TileTypes.DoubleH:
					bool shouldShiftX = ShouldShiftTileX(ti, out Tile bottomTile);
					xPos = boardPos.x + tileRect.width * ti.x + tileRect.width / 2 -
					       (shouldShiftX ? tileShift * bottomTile.Index.floor : 0);
					bool shouldShiftY = ShouldShiftTileY(ti);
					yPos = boardPos.y - tileRect.height * ti.y + tileShift * ti.y -
					       (shouldShiftY ? tileRect.height / 2 : 0);
					break;
				case TileTypes.DoubleV:
					xPos = boardPos.x + tileRect.width * ti.x - tileShift * ti.floor;
					yPos = boardPos.y - tileRect.height * ti.y - tileRect.height / 2 + tileShift * ti.y;
					break;
				default:
					xPos = boardPos.x + tileRect.width * ti.x - tileShift * ti.floor;
					yPos = boardPos.y - tileRect.height * ti.y + tileShift * ti.y;
					break;
			}

			return new Vector2(xPos, yPos);
		}

		private bool ShouldShiftTileX(TileIndex ti, out Tile bottomTile)
		{
			if (ti.floor != 0 && BoardTiles[ti.x, ti.y, ti.floor - 1])
			{
				Tile dummyH = BoardTiles[ti.x + 1, ti.y, ti.floor - 1];
				if (ti.x <= (int) BoardSize.x - 1 && dummyH && dummyH.Type == TileTypes.DummyH)
				{
					bottomTile = BoardTiles[ti.x, ti.y, ti.floor - 1];
					return true;
				}
			}

			bottomTile = null;
			return false;
		}

		private bool ShouldShiftTileY(TileIndex ti)
		{
			if (ti.floor != 0 && BoardTiles[ti.x, ti.y, ti.floor - 1] &&
			    BoardTiles[ti.x, ti.y, ti.floor - 1].Type == TileTypes.DoubleV)
				return true;

			return false;
		}

		public Vector3 BoardSize { get; set; }
		public Tile[,,] BoardTiles { get; set; }
		public GameObject[] Floors { get; set; }
	}
}
