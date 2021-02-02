using LevelEditor;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
	public class GameEditorWindow : EditorWindow
	{
		private GridEditor _gridEditor;
		private BoardEditor _boardEditor;
		private FloorEditor _floorEditor;
		private TileEditor _tileEditor;

		// Board
		private int _boardWidth;
		private int _boardHeight;

		// Floors
		private bool[] _floorsToggle;
		private int _prevFloorsQuantity;
		private bool[] _activeFloors;
		private int _deletedFloor = -1;


		// Visual stuff
		private bool _showScriptsMenu;
		private bool _showBoardMenu = true;
		private bool _showFloorsMenu = true;
		private bool _showTilesMenu = true;


		[MenuItem("Window/GameEditor")]
		public static void ShowWindow()
		{
			GetWindow<GameEditorWindow>("Game Editor");
		}

		public void OnEnable()
		{
			GameObject editor = GameObject.Find("EditorManager");
			_gridEditor = editor.GetComponent<GridEditor>();
			_boardEditor = editor.GetComponent<BoardEditor>();
			_floorEditor = editor.GetComponent<FloorEditor>();
			_tileEditor = editor.GetComponent<TileEditor>();
		}

		public void OnInspectorUpdate()
		{
			Repaint();
		}

		private void SeparateMenu()
		{
			EditorGUILayout.Separator();
			var rect = EditorGUILayout.BeginHorizontal();
			Handles.color = Color.gray;
			Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}

		public void OnGUI()
		{
			EditorGUILayout.Separator();
			ScriptsMenu();
			BoardSpecificationsMenu();
			FloorsMenu();
			TilesMenu();
		}

		private void ScriptsMenu()
		{
			_showScriptsMenu = EditorGUILayout.Foldout(_showScriptsMenu, "Script");
			if (_showScriptsMenu)
			{
				_gridEditor = EditorGUILayout.ObjectField(_gridEditor, typeof(GridEditor), true) as GridEditor;
				_boardEditor = EditorGUILayout.ObjectField(_boardEditor, typeof(BoardEditor), true) as BoardEditor;
				_floorEditor = EditorGUILayout.ObjectField(_floorEditor, typeof(FloorEditor), true) as FloorEditor;
				_tileEditor = EditorGUILayout.ObjectField(_tileEditor, typeof(TileEditor), true) as TileEditor;
			}
		}

		private void BoardSpecificationsMenu()
		{
			SeparateMenu();
			_showBoardMenu = EditorGUILayout.Foldout(_showBoardMenu, "Board");
			if (_showBoardMenu)
			{
				EditorGUILayout.Separator();
				_boardWidth = EditorGUILayout.IntSlider(new GUIContent("Width"), _boardWidth, 1, 8);
				_boardHeight = EditorGUILayout.IntSlider(new GUIContent("Height"), _boardHeight, 1, 7);

				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Create Board"))
					_gridEditor.CreateGrid(_boardWidth, _boardHeight);

				if (GUILayout.Button("Clear Board"))
					_gridEditor.ClearGrid();

				EditorGUILayout.EndHorizontal();
			}
		}

		private void FloorsMenu()
		{
			SeparateMenu();
			_showFloorsMenu = EditorGUILayout.Foldout(_showFloorsMenu, "Floors");
			if (_showFloorsMenu)
			{
				if (GUILayout.Button("Add Floor"))
				{
					_floorEditor.AddNewFloor();
					_floorEditor.SelectedFloor = _floorEditor.SelectedFloor == -1 ?
						_floorEditor.FloorsQuantity - 1 : _floorEditor.SelectedFloor;
				}

				EditorGUILayout.Separator();

				if (_prevFloorsQuantity != _floorEditor.FloorsQuantity)
				{
					_floorsToggle = new bool[_floorEditor.FloorsQuantity];
					if (_activeFloors != null && _activeFloors.Length > 0)
					{
						bool[] prevActiveFloors = _activeFloors;
						_activeFloors = new bool[_floorEditor.FloorsQuantity];
						_activeFloors.Init(true);
						int c = 0;
						for (int p = 0; p < prevActiveFloors.Length; p++)
						{
							if (p == _deletedFloor)
								continue;
							_activeFloors[c] = prevActiveFloors[p];
							c++;
						}

						_deletedFloor = -1;
					}
					else
					{
						_activeFloors = new bool[_floorEditor.FloorsQuantity];
						_activeFloors.Init(true);
					}

					_prevFloorsQuantity = _floorEditor.FloorsQuantity;
				}

				for (int i = 0; i < _floorEditor.FloorsQuantity; i++)
				{
					EditorGUILayout.BeginHorizontal();
					_floorsToggle[i] = GUILayout.Toggle(_floorsToggle[i], $"Floor {i}");
					if (_floorsToggle[i])
						UnselectFloors(i);

					_activeFloors[i] = GUILayout.Toggle(_activeFloors[i], $"Active");
					SelectFloor(i);

					if (GUILayout.Button("Remove"))
					{
						_floorEditor.RemoveFloor(i);
						_floorEditor.SelectedFloor = _floorEditor.SelectedFloor == i ? -1 : _floorEditor.SelectedFloor;
						_deletedFloor = i;
					}

					EditorGUILayout.EndHorizontal();
				}

				if (_floorEditor.SelectedFloor >= 0 && _floorEditor.SelectedFloor < _floorsToggle.Length)
					_floorsToggle[_floorEditor.SelectedFloor] = true;

			}
		}

		private void UnselectFloors(int current)
		{
			for (int i = 0; i < _floorsToggle.Length; i++)
			{
				if (i == current)
					continue;
				_floorsToggle[i] = false;
			}

			_floorEditor.SelectedFloor = current;
		}

		private void SelectFloor(int floorIndex)
		{
			_floorEditor.SelectFloor(floorIndex, _activeFloors[floorIndex]);
		}

		private void TilesMenu()
		{
			SeparateMenu();

			_showTilesMenu = EditorGUILayout.Foldout(_showTilesMenu, "Tiles");
			if (_showTilesMenu)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Normal"))
					CreateTile(1);

				if (GUILayout.Button("Double Horizontal"))
					CreateTile(2);


				if (GUILayout.Button("Double Vertical"))
					CreateTile(4);

				EditorGUILayout.EndHorizontal();
			}
		}

		private void CreateTile(int id)
		{
			_tileEditor.CreateTile(id, _floorEditor.SelectedFloor, new Vector2(_boardWidth, _boardHeight));
		}
	}
}
