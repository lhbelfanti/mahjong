using Board.Tile;
using UnityEngine;

namespace LevelEditor
{
	public class Snap : MonoBehaviour
	{
		private const int Smooth = 10;
		private const float BoundFixer = 0.00001f;

		private RectTransform _gridCell;
		private EditorTile _editorTile;

		private Vector2 _boardSize;
		private Vector2[] _widthBound;
		private Vector2[] _heightBound;
		private bool _boundsSet;

		private int _smoothCounter = Smooth;

		public void OnDrawGizmos()
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
			Rect rect = _gridCell.rect;
			float xOffset = 0f;
			float yOffset = 0f;

			_widthBound = new Vector2[(int) _boardSize.x];
			_heightBound = new Vector2[(int) _boardSize.y];

			switch (_editorTile.type)
			{
				case TileCreator.TileTypes.DoubleH:
					_widthBound = new Vector2[(int) _boardSize.x - 1];
					xOffset = rect.width / 2;
					break;
				case TileCreator.TileTypes.DoubleV:
					_heightBound = new Vector2[(int) _boardSize.y - 1];
					yOffset = rect.height / 2;
					break;
			}

			for (int i = 0; i < _widthBound.Length; i++)
			{
				bool first = i == 0;
				float leftBound = first ? rect.width * i + xOffset: _widthBound[i - 1].y;
				float rightBound = leftBound + (i != _widthBound.Length - 1 ? rect.width + GridEditor.Gap : rect.width);
				_widthBound[i] = new Vector2(leftBound, first ? rightBound : rightBound + BoundFixer);
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
			transform.name = _editorTile.Name();
			return new Vector3(xPos, yPos, pos.z);
		}

		private float RoundX(float x)
		{
			float xPos = x;
			if (xPos < _widthBound[0].x)
			{
				xPos = _widthBound[0].x;
				_editorTile.x = 0;
			}
			else if (xPos > _widthBound[_widthBound.Length - 1].x)
			{
				xPos = _widthBound[_widthBound.Length - 1].x;
				_editorTile.x = _widthBound.Length - 1;
			}

			if (xPos == x)
			{
				for (int i = 0; i < _widthBound.Length; i++)
				{
					Vector2 value = _widthBound[i];
					if (xPos > value.x && xPos < value.y)
					{
						xPos = value.x;
						_editorTile.x = i;
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
				_editorTile.y = 0;
			}
			else if (yPos < _heightBound[_heightBound.Length - 1].x)
			{
				yPos = _heightBound[_heightBound.Length - 1].x;
				_editorTile.y = _heightBound.Length - 1;
			}

			if (yPos == y)
			{
				for (int i = 0; i < _heightBound.Length; i++)
				{
					Vector2 value = _heightBound[i];
					if (yPos < value.x && yPos > value.y)
					{
						yPos = value.x;
						_editorTile.y = i;
						break;
					}
				}
			}
			return yPos;
		}

		public void BoardSize(Vector2 value)
		{
			_boardSize = value;
			SetBounds();
		}

		public RectTransform GridCell
		{
			set => _gridCell = value;
		}

		public EditorTile EditorTile
		{
			set => _editorTile = value;
			get => _editorTile;
		}
	}
}
