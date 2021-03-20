using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace EditorExtensions
{
    [InitializeOnLoad]
    public static class EditorSceneManager
    {
        static EditorSceneManager()
        {
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
            AddToolBar();
        }

        private const string PreloadPath = "Assets/Scenes/PreloadScene.unity";

        private static Texture PlayButton =>
            AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/PlayButton.png");

        private static PlayButtonColors Colors
        {
            get
            {
                var colors = AssetDatabase.LoadAssetAtPath<PlayButtonColors>("Assets/Editor/Customization/PlayButtonSetting.asset");
                if (colors == null)
                {
                    colors = ScriptableObject.CreateInstance<PlayButtonColors>();
                    AssetDatabase.CreateAsset(colors, "Assets/Editor/Customization/PlayButtonSetting.asset");
                }

                return colors;
            }
        }

        private static void AddToolBar()
        {
            if (ToolbarExtender.LeftToolbarGUI.Contains(OpenPreloadSceneButton))
                ToolbarExtender.LeftToolbarGUI.Remove(OpenPreloadSceneButton);
            ToolbarExtender.LeftToolbarGUI.Add(OpenPreloadSceneButton);
        }

        private static void OpenPreloadSceneButton()
        {
            Colors.AnyValueChange += AddToolBar;
            GUILayout.FlexibleSpace();
            var style = ToolbarStyles.commandButtonStyle;
            style.normal.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), Colors.BackGroundColor);
            style.hover.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), Colors.HighlightedColor);
            style.imagePosition = ImagePosition.ImageLeft;
            var playButtonWithColor = TextureCreator.BlendColors((Texture2D) PlayButton, Colors.ButtonColor);
            if (GUILayout.Button(new GUIContent(playButtonWithColor, "Starts From PreLoadScene"), style))
            {
                PlayFromPreLaunchScene();
            }
        }

        private static string Path
        {
            get => PlayerPrefs.GetString("LastPlayedScene", "");
            set => PlayerPrefs.SetString("LastPlayedScene", value);
        }

        private static void PlayFromPreLaunchScene()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            Path = SceneManager.GetActiveScene().path;
            UnityEditor.SceneManagement.EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new [] {SceneManager.GetActiveScene()});
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(PreloadPath);
            EditorApplication.isPlaying = true;
        }

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && !string.IsNullOrEmpty(Path))
            {
                EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(Path);
                Path = null;
            }
        }
    }
}