﻿using System;
using System.Collections.Generic;
using Board;
using Board.Tile;
using LevelEditor;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
	public class LevelsEditorWindow : EditorWindow
	{
		public static bool OnGUIActive;
		public static bool ScriptsLoaded;

		private Vector2 _scrollPos;

		//Scripts
		private bool _showScriptsMenu;
		private GridEditor _gridEditor;
		private BoardEditor _boardEditor;
		private FloorEditor _floorEditor;
		private TileEditor _tileEditor;

		// Importer
		private bool _showImporterMenu = true;

		// Board
		private bool _showBoardMenu = true;
		private bool _gridCreated;

		// Floors
		private bool _showFloorsMenu = true;
		private bool[] _floorsToggle;
		private int _prevFloorsQuantity;
		private bool[] _activeFloors;
		private int _deletedFloor = -1;

		// Tiles
		private bool _showTilesMenu = true;
		private bool[] _tilesSubmenus;
		private int _prevTileSubmenusQuantity;
		private int _deletedTileSubmenu = -1;

		// Validator
		private bool _showValidatorMenu = true;
		private Exporter _exporter;
		private bool _validatorEnabled;
		private int _validatorFillMethod;

		// Exporter
		private bool _showExporterMenu = true;
		private string _defaultPath;
		private string _savePath;
		private int _levelNumber = -1;
		private int _fillMethod;
		private int _choiceIndex;

		[MenuItem("Editor/Open Levels Editor")]
		public static void ShowWindow()
		{
			GetWindow<LevelsEditorWindow>("Levels Editor");
		}

		public void OnEnable()
		{
			if (OnGUIActive)
			{
				GameObject editor = GameObject.Find("EditorManager");
				_gridEditor = editor.GetComponent<GridEditor>();
				_boardEditor = editor.GetComponent<BoardEditor>();
				_floorEditor = editor.GetComponent<FloorEditor>();
				_tileEditor = editor.GetComponent<TileEditor>();
				_defaultPath = $"{Application.dataPath}/Resources/Text/";
				_savePath = _defaultPath;
				ScriptsLoaded = true;
			}
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
			if (!OnGUIActive)
			{
				if (GUILayout.Button("Activate Menus"))
					OnGUIActive = true;
				return;
			}

			if (!ScriptsLoaded)
			{
				OnEnable();
				return;
			}

			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			EditorGUILayout.Separator();

			ScriptsMenu();
			ImporterMenu();
			BoardSpecificationsMenu();
			FloorsMenu();
			TilesMenu();
			ValidatorMenu();
			ExporterMenu();

			EditorGUILayout.Separator();
			EditorGUILayout.EndScrollView();
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

		private void ImporterMenu()
		{
			SeparateMenu();

			_showImporterMenu = EditorGUILayout.Foldout(_showImporterMenu, "Importer");
			if (_showImporterMenu)
			{
				if (GUILayout.Button("Import Level"))
				{
					string path = EditorUtility.OpenFilePanel("Search for a level file", _defaultPath, "json");
					if (path.Length != 0)
					{
						if (_validatorEnabled && EditorUtility.DisplayDialog("Do you want to save the current level?",
							"It will be saved in the editor temporary file and if you want to save it with another " +
							"name you'll have to import it again and save it.", "Yes", "No"))
						{
							_exporter = new Exporter(_boardEditor.Tiles(), _gridEditor, _floorEditor);
							_exporter.Validate();
							if (_exporter.CanBeExported)
							{
								_exporter.SaveTemp(_validatorFillMethod);
								ImportLevel(path);
							}
							else
							{
								EditorUtility.DisplayDialog("Validation error",
									"If you want to save the current level, the errors must be fixed.", "Ok");
							}
						}
						else
							ImportLevel(path);
					}
				}
			}
		}

		private void ImportLevel(string path)
		{
			ClearBoard();
			Importer importer = new Importer(_gridEditor, _boardEditor, _floorEditor, _tileEditor);
			importer.ImportLevel(path);
			_validatorEnabled = true;
		}

		private void BoardSpecificationsMenu()
		{
			SeparateMenu();
			_showBoardMenu = EditorGUILayout.Foldout(_showBoardMenu, "Board");
			if (_showBoardMenu)
			{
				EditorGUILayout.Separator();
				_gridEditor.Width = EditorGUILayout.IntSlider(new GUIContent("Width"), _gridEditor.Width, 1, 8);
				_gridEditor.Height = EditorGUILayout.IntSlider(new GUIContent("Height"), _gridEditor.Height, 1, 7);

				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Create Board"))
				{
					_gridEditor.CreateGrid();
					_gridCreated = true;
				}

				if (GUILayout.Button("Clear Board"))
				{
					if (EditorUtility.DisplayDialog("Are you sure to clear the board?",
						"It will delete all the floors and tiles. All the progress you've not saved will be lost.",
						"Yes", "Cancel"))
					{
						ClearBoard();
					}
				}

				EditorGUILayout.EndHorizontal();
			}
		}

		private void ClearBoard()
		{
			_gridEditor.ClearGrid();
			_gridCreated = false;
			_boardEditor.RemoveAllTiles();
			_floorEditor.RemoveAllFloors();
			InvalidateExporter();
			_validatorEnabled = false;
			Utils.Utils.ClearConsole();
		}

		private void FloorsMenu()
		{
			SeparateMenu();

			_showFloorsMenu = EditorGUILayout.Foldout(_showFloorsMenu, "Floors");
			if (_showFloorsMenu)
			{
				FloorsMenuVisualCalculation();

				for (int i = 0; i < _floorEditor.FloorsQuantity; i++)
				{
					EditorGUILayout.BeginHorizontal();
					_floorsToggle[i] = GUILayout.Toggle(_floorsToggle[i], $"Floor {i}");
					if (_floorsToggle[i])
						UnselectFloors(i);

					_activeFloors[i] = GUILayout.Toggle(_activeFloors[i], "Active");
					SelectFloor(i);

					if (GUILayout.Button("Remove", GUILayout.Width(150)))
					{
						_floorEditor.RemoveFloor(i);
						_boardEditor.RemoveTilesInFloor(i);
						_deletedFloor = i;
						_deletedTileSubmenu = i;
						InvalidateExporter();
					}

					EditorGUILayout.EndHorizontal();
				}

				EditorGUILayout.Separator();

				EditorGUI.BeginDisabledGroup(!_gridCreated);
				if (GUILayout.Button("Add Floor"))
				{
					_floorEditor.AddNewFloor();

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
					CreateTile(TileCreator.TileTypes.Single);

				if (GUILayout.Button("Double Horizontal"))
					CreateTile(TileCreator.TileTypes.DoubleH);

				if (GUILayout.Button("Double Vertical"))
					CreateTile(TileCreator.TileTypes.DoubleV);

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

			for (int i = 0; i < _floorEditor.FloorsQuantity; i++)
			{
				_tilesSubmenus[i] = EditorGUILayout.Foldout(_tilesSubmenus[i], $"Floor {i.ToString()}", foldoutStyle);
				if (_tilesSubmenus[i])
				{
					while (tiles.Count > 0)
					{
						EditorTile et = tiles[0];
						if (et.floor != i)
						{
							tiles.RemoveAt(0);
							continue;
						}

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

		private void CreateTile(TileCreator.TileTypes type)
		{
			GameObject tile = _tileEditor.CreateTile(type, _floorEditor.SelectedFloor);
			EditorTile et = tile.GetComponent<EditorTile>();
			_boardEditor.AddTile(et);

			InvalidateExporter();
			_validatorEnabled = true;
		}

		private void ValidatorMenu()
		{
			SeparateMenu();
			_showValidatorMenu = EditorGUILayout.Foldout(_showValidatorMenu, "Validator");
			if (_showValidatorMenu)
			{
				EditorGUILayout.BeginHorizontal();
				_validatorFillMethod = EditorGUILayout.IntField("Fill Method: ",
					Mathf.Clamp(_validatorFillMethod, 0, Enum.GetNames(typeof(BoardImages.FillMethod)).Length - 1),
					GUILayout.ExpandWidth(false));
				string helpBoxText = _validatorFillMethod == 0 ? "Random" : "By Floor";
				EditorGUILayout.HelpBox(new GUIContent(helpBoxText));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup(!_validatorEnabled);
				if (GUILayout.Button("Validate"))
				{
					_exporter = new Exporter(_boardEditor.Tiles(), _gridEditor, _floorEditor);
					_exporter.Validate();
					if (_exporter.CanBeExported)
					{
						_exporter.SaveTemp(_validatorFillMethod);
						EditorUtility.DisplayDialog("Validation Successful",
							"The level was saved in a temp file.", "Ok");
					}
				}
				EditorGUI.EndDisabledGroup();
				EditorGUILayout.EndHorizontal();
			}
		}

		private void InvalidateExporter()
		{
			if (_exporter != null)
				_exporter.CanBeExported = false;
		}

		private void ExporterMenu()
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
				_levelNumber = EditorGUILayout.IntField(description,
					Mathf.Clamp(_levelNumber, -1,10000),
					GUILayout.ExpandWidth(false));

				EditorGUILayout.BeginHorizontal();
				_fillMethod = EditorGUILayout.IntField("Fill Method: ",
					Mathf.Clamp(_fillMethod, 0, Enum.GetNames(typeof(BoardImages.FillMethod)).Length - 1),
					GUILayout.ExpandWidth(false));
				string helpBoxText = _fillMethod == 0 ? "Random" : "By Floor";
				EditorGUILayout.HelpBox(new GUIContent(helpBoxText));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();

				EditorGUI.BeginDisabledGroup(!_validatorEnabled);
				if (GUILayout.Button("Save"))
				{
					if (_exporter != null)
					{
						if (_savePath == "")
							_savePath = _defaultPath;
						_exporter.Save(_levelNumber, _fillMethod, _savePath);
						EditorUtility.DisplayDialog("Level saved",
							"The level was successfully saved.", "Ok");
					}
					else
					{
						EditorUtility.DisplayDialog("Validation error",
							"The level should be validated before saving it.", "Ok");
					}
				}

				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Separator();
			}
		}
	}
}
