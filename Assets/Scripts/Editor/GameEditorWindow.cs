using System;
using System.Collections.Generic;
using Board;
using Board.Tile;
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

		//Scripts
		private bool _showScriptsMenu;

		// Board
		private bool _showBoardMenu = true;
		private int _boardWidth;
		private int _boardHeight;
		private bool _gridCreated;

		// Floors
		private bool _showFloorsMenu = true;
		private bool[] _floorsToggle;
		private int _prevFloorsQuantity;
		private bool[] _activeFloors;
		private int _deletedFloor = -1;
		private Vector2 _floorsScrollPos;

		// Tiles
		private bool _showTilesMenu = true;
		private bool[] _tilesSubmenus;
		private int _prevTileSubmenusQuantity;
		private int _deletedTileSubmenu = -1;
		private Vector2 _tilesScrollPos;

		// Validator
		private bool _showValidatorMenu = true;
		private Exporter _exporter;
		private bool _validatorEnabled;

		// Exporter
		private bool _showExporterMenu = true;
		private string _defaultPath;
		private string _savePath;
		private int _levelNumber = -1;
		private int _fillMethod;
		private int _choiceIndex;

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
			_defaultPath = Application.dataPath + "/Resources/Text/";
			_savePath = _defaultPath;
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

		private void SeparateSubMenu()
		{
			EditorGUILayout.Separator();
			var rect = EditorGUILayout.BeginHorizontal();
			Handles.color = Color.gray;
			Handles.DrawLine(new Vector2(rect.x + 15, rect.y), new Vector2(rect.width - 15, rect.y));
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
			ExporterMenu();
			SaveMenu();
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
				{
					_gridEditor.CreateGrid(_boardWidth, _boardHeight);
					_gridCreated = true;
				}

				if (GUILayout.Button("Clear Board"))
				{
					if (EditorUtility.DisplayDialog("Are you sure to clear the board?",
						"It will delete all the floors and tiles. All the progress you've not saved will be lost.",
						"Yes", "Cancel"))
					{
						_gridEditor.ClearGrid();
						_gridCreated = false;
						_boardEditor.RemoveAllTiles();
						_floorEditor.RemoveAllFloors();
						InvalidateExporter();
						_validatorEnabled = false;
					}
				}

				EditorGUILayout.EndHorizontal();
			}
		}

		private void FloorsMenu()
		{
			SeparateMenu();
			_showFloorsMenu = EditorGUILayout.Foldout(_showFloorsMenu, "Floors");
			if (_showFloorsMenu)
			{
				FloorsMenuVisualCalculation();

				_floorsScrollPos = EditorGUILayout.BeginScrollView(_floorsScrollPos);
				for (int i = 0; i < _floorEditor.FloorsQuantity; i++)
				{
					EditorGUILayout.BeginHorizontal();
					_floorsToggle[i] = GUILayout.Toggle(_floorsToggle[i], $"Floor {i}");
					if (_floorsToggle[i])
						UnselectFloors(i);

					_activeFloors[i] = GUILayout.Toggle(_activeFloors[i], $"Active");
					SelectFloor(i);

					if (GUILayout.Button("Remove", GUILayout.Width(150)))
					{
						_floorEditor.RemoveFloor(i);
						_floorEditor.SelectedFloor = _floorEditor.SelectedFloor == i ? -1 : _floorEditor.SelectedFloor;
						_boardEditor.RemoveTilesInFloor(i);
						_deletedFloor = i;
						_deletedTileSubmenu = i;
						InvalidateExporter();
					}

					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndScrollView();

				EditorGUILayout.Separator();

				EditorGUI.BeginDisabledGroup(!_gridCreated);
				if (GUILayout.Button("Add Floor"))
				{
					_floorEditor.AddNewFloor();
					_floorEditor.SelectedFloor = _floorEditor.SelectedFloor == -1 ?
						_floorEditor.FloorsQuantity - 1 : _floorEditor.SelectedFloor;
					InvalidateExporter();
				}
				EditorGUI.EndDisabledGroup();

				if (_floorEditor.SelectedFloor >= 0 && _floorEditor.SelectedFloor < _floorsToggle.Length)
					_floorsToggle[_floorEditor.SelectedFloor] = true;

			}
		}

		private void FloorsMenuVisualCalculation()
		{
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
				EditorGUI.BeginDisabledGroup(_floorEditor.SelectedFloor == -1);
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Normal"))
					CreateTile(TileCreator.TileStates.Single);

				if (GUILayout.Button("Double Horizontal"))
					CreateTile(TileCreator.TileStates.DoubleH);

				if (GUILayout.Button("Double Vertical"))
					CreateTile(TileCreator.TileStates.DoubleV);

				EditorGUILayout.EndHorizontal();
				EditorGUI.EndDisabledGroup();

				TilesByFloorMenu();
			}
		}

		private void TilesByFloorMenu()
		{
			TileSubmenusVisualCalculation();

			SeparateSubMenu();

			List<EditorTile> tiles = _boardEditor.Tiles();

			GUILayout.Label($"Tiles Count: {tiles.Count.ToString()}");
			EditorGUILayout.Separator();

			GUIStyle labelStyle = CreateStyleWithMargin(GUI.skin.label, 30, 0);
			GUIStyle buttonStyle = CreateStyleWithMargin (GUI.skin.button, 0, 15);
			GUIStyle foldoutStyle = CreateStyleWithMargin (EditorStyles.foldout,  5, 0);

			_tilesScrollPos = EditorGUILayout.BeginScrollView(_tilesScrollPos);
			for (int i = 0; i < _floorEditor.FloorsQuantity; i++)
			{
				_tilesSubmenus[i] = EditorGUILayout.Foldout(_tilesSubmenus[i], $"Floor {i.ToString()}", foldoutStyle);
				if (_tilesSubmenus[i])
				{
					while (tiles.Count > 0)
					{
						EditorTile et = tiles[0];
						if (et.floor != i)
							break;

						EditorGUILayout.BeginHorizontal();
						GUILayout.Label(et.Name(), labelStyle);

						if (GUILayout.Button("Select", buttonStyle, GUILayout.Width(100)))
							Selection.activeGameObject = et.gameObject;

						if (GUILayout.Button("Remove", buttonStyle, GUILayout.Width(150)))
						{
							_boardEditor.RemoveTile(et);
							InvalidateExporter();
						}

						EditorGUILayout.EndHorizontal();
						tiles.RemoveAt(0);
					}
				}
			}
			EditorGUILayout.EndScrollView();
		}

		private void TileSubmenusVisualCalculation()
		{
			if (_prevTileSubmenusQuantity != _floorEditor.FloorsQuantity)
			{
				if (_tilesSubmenus != null && _tilesSubmenus.Length > 0)
				{
					bool[] prevTileSubmenus = _tilesSubmenus;
					_tilesSubmenus = new bool[_floorEditor.FloorsQuantity];
					_tilesSubmenus.Init(true);
					int c = 0;
					for (int p = 0; p < prevTileSubmenus.Length; p++)
					{
						if (p == _deletedTileSubmenu)
							continue;
						_tilesSubmenus[c] = prevTileSubmenus[p];
						c++;
					}
					_deletedTileSubmenu = -1;
				}
				else
				{
					_tilesSubmenus = new bool[_floorEditor.FloorsQuantity];
					_tilesSubmenus.Init(true);
				}

				_prevTileSubmenusQuantity = _floorEditor.FloorsQuantity;
			}
		}

		private GUIStyle CreateStyleWithMargin(GUIStyle style, int left, int right, int top = 0, int bottom = 0)
		{
			GUIStyle newStyle = new GUIStyle (style);
			newStyle.margin = new RectOffset(left, right, top, bottom);
			return newStyle;
		}

		private void CreateTile(TileCreator.TileStates id)
		{
			GameObject tile = _tileEditor.CreateTile(id, _floorEditor.SelectedFloor, new Vector2(_boardWidth, _boardHeight));
			EditorTile et = tile.GetComponent<EditorTile>();
			_boardEditor.AddTile(et);

			InvalidateExporter();
			_validatorEnabled = true;
		}

		private void ExporterMenu()
		{
			SeparateMenu();
			_showValidatorMenu = EditorGUILayout.Foldout(_showValidatorMenu, "Validator");
			if (_showValidatorMenu)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup(!_validatorEnabled);
				if (GUILayout.Button("Validate"))
				{
					_exporter = new Exporter(_boardEditor.Tiles(),
							new Vector3(_boardWidth, _boardHeight, _floorEditor.FloorsQuantity));
					_exporter.Validate();
				}
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup(_exporter == null || (_exporter != null && !_exporter.canBeExported));
				if (GUILayout.Button("Test Level"))
					_exporter?.SaveTemp();
				EditorGUI.EndDisabledGroup();
				EditorGUILayout.EndHorizontal();
			}
		}

		private void InvalidateExporter()
		{
			if (_exporter != null)
				_exporter.canBeExported = false;
		}

		private void SaveMenu()
		{
			SeparateMenu();

			_showExporterMenu = EditorGUILayout.Foldout(_showExporterMenu, "Exporter");
			if (_showExporterMenu)
			{
				EditorGUILayout.Separator();
				EditorGUI.BeginDisabledGroup(true);
				_savePath = EditorGUILayout.TextField("Save Path: ",_savePath);
				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Separator();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Default"))
					_savePath = _defaultPath;

				if (GUILayout.Button("Open"))
					_savePath = EditorUtility.OpenFolderPanel("Save path", _savePath, "");
				EditorGUILayout.EndHorizontal();

				SeparateSubMenu();

				string description = "Level Number: ";
				EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent(description)).x;
				_levelNumber = EditorGUILayout.IntField(description,
					Mathf.Clamp(_levelNumber, -1,10000),
					GUILayout.ExpandWidth(false));

				EditorGUILayout.BeginHorizontal();
				EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent(description)).x;
				_fillMethod = EditorGUILayout.IntField("Fill Method: ",
					Mathf.Clamp(_fillMethod, 0, Enum.GetNames(typeof(BoardImages.FillMethod)).Length - 1),
					GUILayout.ExpandWidth(false));
				string helpBoxText = _fillMethod == 0 ? "Random" : "By Floor";
				EditorStyles.helpBox.fixedWidth = GUI.skin.label.CalcSize(new GUIContent(helpBoxText)).x;
				EditorGUILayout.HelpBox(new GUIContent(helpBoxText));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();

				if (GUILayout.Button("Save"))
				{
					//TODO: Implement Save
				}

				EditorGUILayout.Separator();
			}
		}
	}
}
