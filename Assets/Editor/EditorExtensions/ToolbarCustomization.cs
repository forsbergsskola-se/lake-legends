using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EditorExtensions
{
    public class ToolbarCustomization : ScriptableObject
    {
        [SerializeField] private bool enabled;
        [SerializeField] private Color buttonOrTextColor = Color.white;
        [SerializeField] private Color backGroundColor = new Color(0f, 0.6f, 0);
        [SerializeField] private Color highlightedColor = new Color(0f, 0.8f, 0);
        [SerializeField] private ScenePathRef favouriteScene1 = new ScenePathRef("Assets/Scenes/JackJ/MainScreen.unity");
        [SerializeField] private ScenePathRef favouriteScene2 = new ScenePathRef("Assets/Scenes/JackJ/JackJ.unity");
        [SerializeField] private ScenePathRef favouriteScene3 = new ScenePathRef("Assets/Scenes/PreloadScene.unity");

        public Dictionary<string, int> FavouriteScenes
        {
            get
            {
                var faveScenes = new Dictionary<string, int>();
                if (!string.IsNullOrEmpty(favouriteScene1.path))
                {
                    faveScenes.Add(favouriteScene1.path, 1);
                }
                if (!string.IsNullOrEmpty(favouriteScene2.path))
                {
                    faveScenes.Add(favouriteScene2.path, 2);
                }
                if (!string.IsNullOrEmpty(favouriteScene3.path))
                {
                    faveScenes.Add(favouriteScene3.path, 3);
                }
                return faveScenes;
            }
        }

        public Color HighlightedColor => highlightedColor;
        
        public Color BackGroundColor => backGroundColor;

        public Color ButtonOrTextColor => buttonOrTextColor;

        public bool Enabled => enabled;

        public void Validate()
        {
            var scenePaths = EditorSceneManager.UpdateValidPaths().ToArray();
            favouriteScene1.Validate(scenePaths);
            favouriteScene2.Validate(scenePaths);
            favouriteScene3.Validate(scenePaths);
        }
    }
}