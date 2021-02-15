using UnityEngine;

namespace LevelEditor
{
	public class GridEditor : MonoBehaviour
	{
		[SerializeField] private GameObject gridCell;
		[SerializeField] private Transform grid;

		public static readonly float Gap = 0.1f;

		public void CreateGrid()
		{
			ClearGrid();

			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					Rect boardRect = gridCell.GetComponent<RectTransform>().rect;
					GameObject cell = Instantiate(gridCell,
						new Vector3(i * (boardRect.width + Gap), j * (-boardRect.height - Gap), 0),
						gridCell.transform.rotation);
					cell.transform.SetParent(grid);
					cell.name = $"{i.ToString()}x{j.ToString()}";
				}
			}
		}

		public void ClearGrid()
		{
			while (grid.childCount > 0)
				foreach(Transform child in grid)
					DestroyImmediate(child.gameObject);
		}

		public int Width { get; set; }
		public int Height { get; set; }
	}
}
