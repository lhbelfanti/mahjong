using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class Snap : MonoBehaviour
	{
		public TileCreator.TileStates tileState;

		private void OnDrawGizmos()
		{
			switch (tileState)
			{
				case TileCreator.TileStates.Single:
					SnapToGridSingle();
					break;
				case TileCreator.TileStates.DoubleH:
					SnapToGridDoubleH();
					break;
				case TileCreator.TileStates.DoubleV:
					SnapToGridDoubleV();
					break;
			}
		}

		private void SnapToGridSingle()
		{
			Vector3 position = new Vector3(
				Mathf.RoundToInt(transform.position.x),
				Mathf.RoundToInt(transform.position.y),
				Mathf.RoundToInt(transform.position.z)
			);

			transform.position = position;
		}

		private void SnapToGridDoubleH()
		{
			Vector3 position = new Vector3(
				Mathf.RoundToInt(transform.position.x),
				Mathf.RoundToInt(transform.position.y),
				Mathf.RoundToInt(transform.position.z)
			);

			transform.position = position;
		}

		private void SnapToGridDoubleV()
		{
			Vector3 position = new Vector3(
				Mathf.RoundToInt(transform.position.x),
				Mathf.RoundToInt(transform.position.y),
				Mathf.RoundToInt(transform.position.z)
			);

			transform.position = position;
		}
	}
}
