using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace EditorExtensions
{
    [InitializeOnLoad]
    public static class EditorSceneManager
    {
        private static IEnumerable<string> _validPaths;

        static EditorSceneManager()
        {
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
            AddToolBar();
        }

        private const string PreloadPath = "Assets/Scenes/PreloadScene.unity";

        private static IEnumerable<string> ValidPaths => _validPaths ??= AssetQuerying.FindPathsForAllScenes("Scenes");

        public static IEnumerable<string> UpdateValidPaths()
        {
            return _validPaths = AssetQuerying.FindPathsForAllScenes("Scenes");
        }

        private static bool IsValidPath(string path)
        {
            return !string.IsNullOrEmpty(path) && ValidPaths.Any(newPath => newPath == path);
        }

        private static ToolbarCustomization Settings
        {
            get
            {
                var settings = AssetDatabase.LoadAssetAtPath<ToolbarCustomization>("Assets/Editor/Customization/ToolbarSettings.asset");
                if (settings == null)
                {
                    settings = ScriptableObject.CreateInstance<ToolbarCustomization>();
                    AssetDatabase.CreateAsset(settings, "Assets/Editor/Customization/ToolbarSettings.asset");
                }

                return settings;
            }
        }

        private static void AddToolBar()
        {
            ToolbarExtender.RightToolbarGUI.Add(RightButtons);
            ToolbarExtender.LeftToolbarGUI.Add(LeftButtons);
        }

        private static void RightButtons()
        {
            if (GUILayout.Button(new GUIContent(Settings.SettingsWheelColored, "Open Settings"), Settings.GuiStyle))
            {
                Selection.objects = new Object[] {Settings};
            }
            if (!Settings.Enabled)
                return;
            foreach (var favouriteScene in Settings.ObjectShortCuts)
            {
                if (GUILayout.Button(new GUIContent($"{favouriteScene.Value}", $"Shortcut For {favouriteScene.Key.name}"), Settings.GuiStyle))
                    Selection.objects = new Object[] {favouriteScene.Key};
            }
            GUILayout.FlexibleSpace();
        }

        private static void LeftButtons()
        {
            if (!Settings.Enabled)
                return;
            GUILayout.FlexibleSpace();
            foreach (var favouriteScene in Settings.FavouriteScenes.Where(favouriteScene => IsValidPath(favouriteScene.Key)))
            {
                if (GUILayout.Button(new GUIContent($"{favouriteScene.Value}", $"Opens {Path.GetFileNameWithoutExtension(favouriteScene.Key)}"), Settings.GuiStyle))
                    LoadSceneNamed(favouriteScene.Key);
            }
            
            if (GUILayout.Button(new GUIContent(Settings.PlayButtonColored, "Starts From PreLoadScene"), Settings.GuiStyle))
            {
                PlayFromPreLaunchScene();
            }
        }

        private static string LastPlayedPath
        {
            get => PlayerPrefs.GetString("LastPlayedScene", "");
            set => PlayerPrefs.SetString("LastPlayedScene", value);
        }

        private static void LoadSceneNamed(string path)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }
            
            UnityEditor.SceneManagement.EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new [] {SceneManager.GetActiveScene()});
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/"+path);
        }

        private static void PlayFromPreLaunchScene()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            LastPlayedPath = SceneManager.GetActiveScene().path;
            UnityEditor.SceneManagement.EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new [] {SceneManager.GetActiveScene()});
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(PreloadPath);
            EditorApplication.isPlaying = true;
        }

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && !string.IsNullOrEmpty(LastPlayedPath))
            {
                EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LastPlayedPath);
                LastPlayedPath = null;
            }
        }
    }
}