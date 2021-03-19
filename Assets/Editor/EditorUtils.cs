using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public static class EditorUtils
    {
        static EditorUtils()
        {
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        }

        private static string Path
        {
            get => PlayerPrefs.GetString("LastPlayedScene", "");
            set => PlayerPrefs.SetString("LastPlayedScene", value);
        }
        
        [MenuItem("Edit/Play-Unplay, PreloadScene")]
        public static void PlayFromPreLaunchScene()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            Path = SceneManager.GetActiveScene().path;
            EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new [] {SceneManager.GetActiveScene()});
            EditorSceneManager.OpenScene("Assets/Scenes/PreloadScene.unity");
            EditorApplication.isPlaying = true;
        }

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && !string.IsNullOrEmpty(Path))
            {
                EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
                EditorSceneManager.OpenScene(Path);
                Path = null;
            }
        }
    }
}