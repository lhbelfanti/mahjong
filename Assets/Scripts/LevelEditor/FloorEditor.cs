using UnityEngine;

namespace LevelEditor
{
	public class FloorEditor : MonoBehaviour
	{
		[SerializeField] private Transform floorsContainer;

		private int _floorsQuantity;

		public void AddNewFloor()
		{
			GameObject floor = new GameObject($"Floor {_floorsQuantity.ToString()}");
			floor.transform.parent = floorsContainer;
			_floorsQuantity++;

			SelectedFloor = SelectedFloor == -1 ? FloorsQuantity - 1 : SelectedFloor;
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

			SelectedFloor = SelectedFloor == floorIndex ? -1 : SelectedFloor;
		}

		public void RemoveAllFloors()
		{
			for (int i = 0; i < _floorsQuantity; i++)
			{
				Transform floor = floorsContainer.Find($"Floor {i.ToString()}");
				DestroyImmediate(floor.gameObject);
			}

			_floorsQuantity = 0;
			SelectedFloor = -1;
		}

		public void SelectFloor(int floorIndex, bool value)
		{
			Transform floor = floorsContainer.Find($"Floor {floorIndex.ToString()}");
			floor.gameObject.SetActive(value);
		}

		public int FloorsQuantity => _floorsQuantity;

		public int SelectedFloor { get; set; } = -1;
	}
}
