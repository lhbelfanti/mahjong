using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
	[InitializeOnLoad]
	static class SceneAutoLoader
	{
		static SceneAutoLoader()
		{
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
		}

		// Menu items to select the "master" scene and control whether or not to load it.
		[MenuItem("Editor/Scene Autoload/Select Master Scene...")]
		private static void SelectMasterScene()
		{
			string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
			masterScene = masterScene.Replace(Application.dataPath, "Assets");
			if (!string.IsNullOrEmpty(masterScene))
			{
				MasterScene = masterScene;
				LoadMasterOnPlay = true;
			}
		}

		[MenuItem("Editor/Scene Autoload/Load Master On Play", true)]
		private static bool ShowLoadMasterOnPlay()
		{
			return !LoadMasterOnPlay;
		}
		[MenuItem("Editor/Scene Autoload/Load Master On Play")]
		private static void EnableLoadMasterOnPlay()
		{
			LoadMasterOnPlay = true;
		}

		[MenuItem("Editor/Scene Autoload/Don't Load Master On Play", true)]
		private static bool ShowDontLoadMasterOnPlay()
		{
			return LoadMasterOnPlay;
		}
		[MenuItem("Editor/Scene Autoload/Don't Load Master On Play")]
		private static void DisableLoadMasterOnPlay()
		{
			LoadMasterOnPlay = false;
		}

		// Play mode change callback handles the scene load/reload.
		private static void OnPlayModeChanged(PlayModeStateChange state)
		{
			if (!LoadMasterOnPlay)
			{
				return;
			}

			if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
			{
				// User pressed play -- autoload master scene.
				PreviousScene = EditorSceneManager.GetActiveScene().path;

				try
				{
					LevelsEditorWindow.OnGUIActive = false;
					LevelsEditorWindow.ScriptsLoaded = false;
					EditorSceneManager.OpenScene(MasterScene);
				}
				catch
				{
					Debug.LogError($"error: scene not found: {MasterScene}");
					EditorApplication.isPlaying = false;

				}
			}

			// isPlaying check required because cannot OpenScene while playing
			if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
			{
				// User pressed stop -- reload previous scene.
				try
				{
					LevelsEditorWindow.OnGUIActive = true;
					EditorSceneManager.OpenScene(PreviousScene);
				}
				catch
				{
					Debug.LogError($"error: scene not found: {PreviousScene}");
				}
			}
		}

		// Properties are remembered as editor preferences.
		private const string CEditorPrefLoadMasterOnPlay = "SceneAutoLoader.LoadMasterOnPlay";
		private const string CEditorPrefMasterScene = "SceneAutoLoader.MasterScene";
		private const string CEditorPrefPreviousScene = "SceneAutoLoader.PreviousScene";

		private static bool LoadMasterOnPlay
		{
			get => EditorPrefs.GetBool(CEditorPrefLoadMasterOnPlay, false);
			set => EditorPrefs.SetBool(CEditorPrefLoadMasterOnPlay, value);
		}

		private static string MasterScene
		{
			get => EditorPrefs.GetString(CEditorPrefMasterScene, "Master.unity");
			set => EditorPrefs.SetString(CEditorPrefMasterScene, value);
		}

		private static string PreviousScene
		{
			get => EditorPrefs.GetString(CEditorPrefPreviousScene, EditorSceneManager.GetActiveScene().path);
			set => EditorPrefs.SetString(CEditorPrefPreviousScene, value);
		}
	}
}
