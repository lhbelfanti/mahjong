using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class Snap : MonoBehaviour
	{
		[SerializeField] private RectTransform gridCell;

		private TileCreator.TileStates _tileState;
		private Vector2 _boardSize;
		private Vector2[] _widthBound;
		private Vector2[] _heightBound;
		private bool _boundsSet;

		private int _smoothCounter;
		private const int Smooth = 10;

		private void OnDrawGizmos()
		{
			if (!Application.isPlaying && transform.hasChanged && _boundsSet && _smoothCounter == Smooth)
			{
				_smoothCounter = 0;
				switch (_tileState)
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

			_smoothCounter++;
		}

		private void SnapToGridSingle()
		{
			transform.position = RoundToNearestCell(transform.position);
		}

		private void SnapToGridDoubleH()
		{
			// TODO
		}

		private void SnapToGridDoubleV()
		{
			// TODO
		}

		public TileCreator.TileStates TileState
		{
			set => _tileState = value;
		}

		public void BoardSize(Vector2 value)
		{
			_boardSize = value;
			SetBounds();
		}

		private void SetBounds()
		{
			Rect rect = gridCell.rect;
			_widthBound = new Vector2[(int) _boardSize.x];
			_heightBound = new Vector2[(int) _boardSize.y];

			for (int i = 0; i < _widthBound.Length; i++)
			{
				float xValue = i == 0 ? rect.width * i : _widthBound[i - 1].y;
				float yValue = xValue + (i != _widthBound.Length - 1 ? rect.width + GridEditor.Gap : rect.width);
				_widthBound[i] = new Vector2(xValue, yValue);
			}

			for (int i = 0; i < _heightBound.Length; i++)
			{
				float xValue = i == 0 ? -(rect.height * i) : _heightBound[i - 1].y;
				float yValue = xValue + (i != _heightBound.Length - 1 ? -(rect.height + GridEditor.Gap) : -rect.height);
				_heightBound[i] = new Vector2(xValue, yValue);
			}

			_boundsSet = true;
		}

		private Vector3 RoundToNearestCell(Vector3 pos)
		{
			float xPos = pos.x;
			if (xPos < _widthBound[0].x)
				xPos = _widthBound[0].x;
			else if (xPos > _widthBound[_widthBound.Length - 1].x)
				xPos = _widthBound[_widthBound.Length - 1].x;

			if (xPos == pos.x)
			{
				for (int i = 0; i < _widthBound.Length; i++)
				{
					Vector2 value = _widthBound[i];
					if (xPos > value.x && xPos < value.y)
						xPos = value.x;
				}
			}

			float yPos = pos.y;
			if (yPos > _heightBound[0].x)
				yPos = _heightBound[0].x;
			else if (yPos < _heightBound[_heightBound.Length - 1].x)
				yPos = _heightBound[_heightBound.Length - 1].x;

			if (yPos == pos.y)
			{
				for (int i = 0; i < _heightBound.Length; i++)
				{
					Vector2 value = _heightBound[i];
					if (yPos < value.x && yPos > value.y)
						yPos = value.x;
				}
			}

			return new Vector3(xPos, yPos, pos.z);
		}
	}
}
