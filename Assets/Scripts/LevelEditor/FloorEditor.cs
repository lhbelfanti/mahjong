using UnityEngine;

namespace LevelEditor
{
	public class FloorEditor : MonoBehaviour
	{
		[SerializeField] private Transform floorsContainer;

		private int _floorsQuantity = 0;

		public void AddNewFloor()
		{
			GameObject floor = new GameObject($"Floor {_floorsQuantity.ToString()}");
			floor.transform.parent = floorsContainer;
			_floorsQuantity++;
		}

		public void RemoveFloor(int floorIndex)
		{
			Transform floor = floorsContainer.Find($"Floor {floorIndex.ToString()}");
			DestroyImmediate(floor.gameObject);

			if (floorIndex < _floorsQuantity)
			{
				for (int i = floorIndex + 1; i < _floorsQuantity; i++)
				{
					Transform f = floorsContainer.Find($"Floor {i.ToString()}");
					f.name = $"Floor {(i - 1).ToString()}";
				}
			}

			_floorsQuantity--;
		}

		public int FloorsQuantity => _floorsQuantity;
	}
}
