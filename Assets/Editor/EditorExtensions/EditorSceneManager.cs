using UnityEditor;
using UnityEditor.SceneManagement;
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
            ToolbarExtender.LeftToolbarGUI.Add(OpenPreloadSceneButton);
        }

        private const string PreloadPath = "Assets/Scenes/PreloadScene.unity";

        private static Texture PlayButton =>
            AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Images/PlayButton.png");

        private static void OpenPreloadSceneButton()
        {
            GUILayout.FlexibleSpace();
            var style = ToolbarStyles.commandButtonStyle;
            style.normal.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), new Color(0f, 0.6f, 0));
            //style.normal.background = GetTextureFromGreyScale(0.22f);
            style.hover.background = TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), new Color(0f, 0.8f, 0));
            //style.hover.background = GetTextureFromGreyScale(0.3f);
            style.imagePosition = ImagePosition.ImageLeft;
            if (GUILayout.Button(new GUIContent(PlayButton, "Starts From PreLoadScene"), style))
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