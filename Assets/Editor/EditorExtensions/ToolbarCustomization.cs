using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorExtensions
{
    public class ToolbarCustomization : ScriptableObject
    {
        [SerializeField] private bool enabled;
        [SerializeField] private Color buttonOrTextColor = Color.white;
        [SerializeField] private Color backGroundColor = new Color(0f, 0.6f, 0);
        [SerializeField] private Color highlightedColor = new Color(0f, 0.8f, 0);
        [SerializeField] private ScenePathRef favouriteScene1 = new ScenePathRef(@"Scenes\JackJ\MainScreen.unity");
        [SerializeField] private ScenePathRef favouriteScene2 = new ScenePathRef(@"Scenes\JackJ\JackJ.unity");
        [SerializeField] private ScenePathRef favouriteScene3 = new ScenePathRef(@"Scenes\PreloadScene.unity");
        [SerializeField] private Object assetShortCut1;
        [SerializeField] private Object assetShortCut2;
        [SerializeField] private Object assetShortCut3;
        private GUIStyle guiStyle;

        private void Awake()
        {
            if (assetShortCut1 == null)
                assetShortCut1 = AssetQuerying.FindAssetAtPath<Object>("Resources/Global Item Index.asset");
            if (assetShortCut2 == null)
                assetShortCut2 = AssetQuerying.FindAssetAtPath<Object>("ScriptableObjects");
            if (assetShortCut3 == null)
                assetShortCut3 = AssetQuerying.FindAssetAtPath<Object>("Scripts");
        }

        private static Texture PlayButton =>
            AssetQuerying.FindAssetAtPath<Texture>("Editor/Images/PlayButton.png");
        
        private static Texture SettingsWheel =>
            AssetQuerying.FindAssetAtPath<Texture>("Editor/Images/SettingsWheel.png");

        public Texture SettingsWheelColored
        {
            get
            {
                var settingsWheelColored =
                    AssetQuerying.FindAssetAtPath<Texture>("Editor/Images/SettingsWheelColored.png");
                if (settingsWheelColored == null)
                {
                    var settingsWheel =
                        TextureCreator.ReplaceNonTransparentPixels((Texture2D) SettingsWheel, ButtonOrTextColor);
                    TextureCreator.EncodeAndCreateAsset(settingsWheel as Texture2D,
                        "Editor/Images/SettingsWheelColored.png");
                    return settingsWheel;
                }

                return settingsWheelColored;
            }
        }

        public Texture PlayButtonColored
        {
            get
            {
                var playButtonColored =
                    AssetQuerying.FindAssetAtPath<Texture>("Editor/Images/PlayButtonColored.png");;
                if (playButtonColored == null)
                {
                    var playButtonColor = TextureCreator.BlendColors((Texture2D) PlayButton, ButtonOrTextColor);
                    TextureCreator.EncodeAndCreateAsset(playButtonColor as Texture2D,
                        "Editor/Images/PlayButtonColored.png");
                    return playButtonColor;
                }

                return playButtonColored;
            }
        }

        public GUIStyle GuiStyle
        {
            get
            {
                if (guiStyle != null) 
                    return guiStyle;
                var style = ToolbarStyles.commandButtonStyle;
                style.normal.background =
                    TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), BackGroundColor);
                style.hover.background =
                    TextureCreator.GetTextureOfColor(new Vector2Int(30, 20), HighlightedColor);
                style.imagePosition = ImagePosition.ImageLeft;
                style.normal.textColor = ButtonOrTextColor;
                guiStyle = style;

                return guiStyle;
            }
            private set => guiStyle = value;
        }

        public Dictionary<Object, int> ObjectShortCuts
        {
            get
            {
                var shortcutObjects = new Dictionary<Object, int>();
                if (assetShortCut1 != null)
                {
                    shortcutObjects.Add(assetShortCut1, 1);
                }
                if (assetShortCut2 != null)
                {
                    shortcutObjects.Add(assetShortCut2, 2);
                }
                if (assetShortCut3 != null)
                {
                    shortcutObjects.Add(assetShortCut3, 3);
                }
                return shortcutObjects;
            }
        }

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

        public void UpdateSettings()
        {
            GuiStyle = null;
            AssetQuerying.DeleteFileAtRelativePath("Editor/Images/SettingsWheelColored.png");
            AssetQuerying.DeleteFileAtRelativePath("Editor/Images/PlayButtonColored.png");
        }

        private Color HighlightedColor => highlightedColor;

        private Color BackGroundColor => backGroundColor;

        private Color ButtonOrTextColor => buttonOrTextColor;

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