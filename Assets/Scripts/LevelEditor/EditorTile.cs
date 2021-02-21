using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class EditorTile : MonoBehaviour
	{
		[HideInInspector] public int x;
		[HideInInspector] public int y;
		[HideInInspector] public int floor;
		[HideInInspector] public TileCreator.TileTypes type;

		public string Name()
		{
			string newName = $"{x.ToString()}x{y.ToString()} - ";
			switch (type)
			{
				case TileCreator.TileTypes.Single:
					newName += "Single";
					break;
				case TileCreator.TileTypes.DoubleH:
					newName += "DoubleH";
					break;
				case TileCreator.TileTypes.DoubleV:
					newName += "DoubleV";
					break;
			}

			return newName;
		}

		public void SetName()
		{
			gameObject.name = Name();
		}

		public void SetNewPosition()
		{
			GridTile gridTile = GridEditor.SelectedCell;
			x = gridTile.x;
			y = gridTile.y;
			SetPosition(gridTile);
			GridEditor.ChangeSelectedCell(null);
			SetName();
		}

		private void SetPosition(GridTile gridTile)
		{
			Rect gridRect = gridTile.GetComponent<RectTransform>().rect;
			Vector3 newPosition = gridTile.transform.position;
			newPosition.z = transform.position.z;
			switch (type)
			{
				case TileCreator.TileTypes.DoubleH:
					newPosition.x += gridRect.width / 2 + GridEditor.Gap;
					break;
				case TileCreator.TileTypes.DoubleV:
					newPosition.y -= gridRect.height / 2 + GridEditor.Gap;
					break;
			}
			transform.position = newPosition;
		}

		public GridEditor GridEditor { get; set; }
	}
}
