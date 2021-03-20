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

        private static Texture PlayButton =>
            AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/PlayButton.png");
        
        private static Texture SettingsWheel =>
            AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/SettingsWheel.png");

        private static IEnumerable<string> ValidPaths => _validPaths ??= AssetQuerying.FindPathsForAllAssetsWithFileExtension(".unity");

        public static IEnumerable<string> UpdateValidPaths()
        {
            return _validPaths = AssetQuerying.FindPathsForAllAssetsWithFileExtension(".unity");
        }

        private static bool IsValidPath(string path)
        {
            return !string.IsNullOrEmpty(path) && ValidPaths.Any(newPath => newPath == path);
        }

        private static ToolbarCustomization Settings
        {
            get
            {
                var settings = AssetDatabase.LoadAssetAtPath<ToolbarCustomization>("Assets/Editor/Customization/PlayButtonSetting.asset");
                if (settings == null)
                {
                    settings = ScriptableObject.CreateInstance<ToolbarCustomization>();
                    AssetDatabase.CreateAsset(settings, "Assets/Editor/Customization/PlayButtonSetting.asset");
                }

                return settings;
            }
        }

        private static void AddToolBar()
        {
            ToolbarExtender.RightToolbarGUI.Add(ShowSettings);
            ToolbarExtender.LeftToolbarGUI.Add(OpenPreloadSceneButton);
        }

        private static void ShowSettings()
        {
            var style = ToolbarStyles.commandButtonStyle;
            style.normal.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), Settings.BackGroundColor);
            style.hover.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), Settings.HighlightedColor);
            style.imagePosition = ImagePosition.ImageLeft;
            style.normal.textColor = Settings.ButtonOrTextColor;
            var settingsTexture = TextureCreator.ReplaceNonTransparentPixels((Texture2D) SettingsWheel, Settings.ButtonOrTextColor);
            if (GUILayout.Button(new GUIContent(settingsTexture, "Open Settings"), style))
            {
                Selection.objects = new Object[] {Settings};
            }
            GUILayout.FlexibleSpace();
        }

        private static void OpenPreloadSceneButton()
        {
            if (!Settings.Enabled)
                return;
            GUILayout.FlexibleSpace();
            var style = ToolbarStyles.commandButtonStyle;
            style.normal.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), Settings.BackGroundColor);
            style.hover.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), Settings.HighlightedColor);
            style.imagePosition = ImagePosition.ImageLeft;
            style.normal.textColor = Settings.ButtonOrTextColor;
            var playButtonWithColor = TextureCreator.BlendColors((Texture2D) PlayButton, Settings.ButtonOrTextColor);
            foreach (var favouriteScene in Settings.FavouriteScenes.Where(favouriteScene => IsValidPath(favouriteScene.Key)))
            {
                if (GUILayout.Button(new GUIContent($"{favouriteScene.Value}", $"Opens {Path.GetFileNameWithoutExtension(favouriteScene.Key)}"), style))
                    LoadSceneNamed(favouriteScene.Key);
            }
            
            if (GUILayout.Button(new GUIContent(playButtonWithColor, "Starts From PreLoadScene"), style))
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
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);
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