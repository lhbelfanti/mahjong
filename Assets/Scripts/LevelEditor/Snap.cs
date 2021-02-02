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

		private Renamer _renamer;
		private int _xIndex;
		private int _yIndex;

		private int _smoothCounter;
		private const int Smooth = 10;

		private void OnDrawGizmos()
		{
			if (_boundsSet && _smoothCounter == Smooth)
			{
				_smoothCounter = 0;
				transform.position = RoundToNearestCell(transform.position);
			}

			_smoothCounter++;
		}

		private void SetBounds()
		{
			Rect rect = gridCell.rect;
			float xOffset = 0f;
			float yOffset = 0f;

			_widthBound = new Vector2[(int) _boardSize.x];
			_heightBound = new Vector2[(int) _boardSize.y];

			switch (_tileState)
			{
				case TileCreator.TileStates.DoubleH:
					_widthBound = new Vector2[(int) _boardSize.x - 1];
					xOffset = rect.width / 2;
					break;
				case TileCreator.TileStates.DoubleV:
					_heightBound = new Vector2[(int) _boardSize.y - 1];
					yOffset = rect.height / 2;
					break;
			}

			for (int i = 0; i < _widthBound.Length; i++)
			{
				float leftBound = i == 0 ? rect.width * i + xOffset: _widthBound[i - 1].y;
				float rightBound = leftBound + (i != _widthBound.Length - 1 ? rect.width + GridEditor.Gap : rect.width);
				_widthBound[i] = new Vector2(leftBound, rightBound);
			}

			for (int i = 0; i < _heightBound.Length; i++)
			{
				float topBound = i == 0 ? -(rect.height * i) - yOffset : _heightBound[i - 1].y;
				float bottomBound = topBound + (i != _heightBound.Length - 1 ? -(rect.height + GridEditor.Gap) : -rect.height);
				_heightBound[i] = new Vector2(topBound, bottomBound);
			}

			_boundsSet = true;
		}

		private Vector3 RoundToNearestCell(Vector3 pos)
		{
			float xPos = RoundX(pos.x);
			float yPos = RoundY(pos.y);
			_renamer.Rename(_tileState, _xIndex, _yIndex);
			return new Vector3(xPos, yPos, pos.z);
		}

		private float RoundX(float x)
		{
			float xPos = x;
			if (xPos < _widthBound[0].x)
			{
				xPos = _widthBound[0].x;
				_xIndex = 0;
			}
			else if (xPos > _widthBound[_widthBound.Length - 1].x)
			{
				xPos = _widthBound[_widthBound.Length - 1].x;
				_xIndex = _widthBound.Length - 1;
			}

			if (xPos == x)
			{
				for (int i = 0; i < _widthBound.Length; i++)
				{
					Vector2 value = _widthBound[i];
					if (xPos > value.x && xPos < value.y)
					{
						xPos = value.x;
						_xIndex = i;
						break;
					}
				}
			}
			return xPos;
		}

		private float RoundY(float y)
		{
			float yPos = y;
			if (yPos > _heightBound[0].x)
			{
				yPos = _heightBound[0].x;
				_yIndex = 0;
			}
			else if (yPos < _heightBound[_heightBound.Length - 1].x)
			{
				yPos = _heightBound[_heightBound.Length - 1].x;
				_yIndex = _heightBound.Length - 1;
			}

			if (yPos == y)
			{
				for (int i = 0; i < _heightBound.Length; i++)
				{
					Vector2 value = _heightBound[i];
					if (yPos < value.x && yPos > value.y)
					{
						yPos = value.x;
						_yIndex = i;
						break;
					}
				}
			}
			return yPos;
		}

		public TileCreator.TileStates TileState
		{
			set => _tileState = value;
		}

		public Renamer Renamer
		{
			set => _renamer = value;
		}

		public void BoardSize(Vector2 value)
		{
			_boardSize = value;
			SetBounds();
		}
	}
}
