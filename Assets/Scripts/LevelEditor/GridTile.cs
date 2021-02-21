using UnityEngine;

namespace LevelEditor
{
	public class GridTile : MonoBehaviour
	{
		[HideInInspector] public int x;
		[HideInInspector] public int y;

		[SerializeField] private Color selected;
		[SerializeField] private Color unselected;

		private GridEditor _gridEditor;
		private Material _material;

		public void Select()
		{
			_gridEditor.ChangeSelectedCell(this);
			_material.color = selected;
		}

		public void Unselect()
		{
			_material.color = unselected;
		}

		public void Initialize(GridEditor gridEditor)
		{
			_gridEditor = gridEditor;

			Renderer r = GetComponent<Renderer>();
			_material = new Material(r.sharedMaterial);
			r.sharedMaterial = _material;

			Unselect();
		}
	}
}
