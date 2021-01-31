using LevelEditor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class GameEditorWindow : EditorWindow
	{
		private GridEditor _gridEditor;
		private BoardEditor _boardEditor;
		private FloorEditor _floorEditor;
		private TileEditor _tileEditor;

		// Board Specifications
		private int _boardWidth;
		private int _boardHeight;


		// Visual stuff
		private bool _showScriptsMenu;
		private bool _showBoardMenu = true;
		private bool _showFloorsMenu = true;
		private bool _showTilesMenu = true;
		private bool[] _floorsToggle;
		private int _prevFloorsQuantity;
		private int _selectedFloor = -1;


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
					_selectedFloor = _selectedFloor == -1 ? _floorEditor.FloorsQuantity - 1 : _selectedFloor;
				}

				EditorGUILayout.Separator();

				if (_prevFloorsQuantity != _floorEditor.FloorsQuantity)
				{
					_floorsToggle = new bool[_floorEditor.FloorsQuantity];
					_prevFloorsQuantity = _floorEditor.FloorsQuantity;
				}

				for (int i = 0; i < _floorEditor.FloorsQuantity; i++)
				{
					EditorGUILayout.BeginHorizontal();
					_floorsToggle[i] = GUILayout.Toggle(_floorsToggle[i], $"Floor {i}");
					if (_floorsToggle[i])
						UnselectFloors(i);

					if (GUILayout.Button("Remove"))
					{
						_floorEditor.RemoveFloor(i);
						_selectedFloor = _selectedFloor == i ? -1 : _selectedFloor;
					}

					EditorGUILayout.EndHorizontal();
				}

				if (_selectedFloor >= 0 && _selectedFloor < _floorsToggle.Length)
					_floorsToggle[_selectedFloor] = true;
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

			_selectedFloor = current;
		}

		private void TilesMenu()
		{
			SeparateMenu();

			_showTilesMenu = EditorGUILayout.Foldout(_showTilesMenu, "Tiles");
			if (_showTilesMenu)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Normal"))
					CreateTile(0, 1);

				if (GUILayout.Button("Double Horizontal"))
					CreateTile(1, 2);


				if (GUILayout.Button("Double Vertical"))
					CreateTile(2, 4);

				EditorGUILayout.EndHorizontal();
			}
		}

		private void CreateTile(int index, int id)
		{
			_tileEditor.CreateTile(id);
		}
	}
}
