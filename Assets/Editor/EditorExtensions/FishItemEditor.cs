using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fish;
using Items;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class FishItemEditor : EditorWindow
    {
        private FishType fishType;
        private const string Folder = "Assets/ScriptableObjects/Fish/Items";
        [MenuItem("Window/Custom Editors/Fish Item Creator")]
        private static void ShowWindow()
        {
            GetWindow(typeof(FishItemEditor), false, "Fish Item Creator");
        }
        
        private void OnGUI()
        {
            EditorGUILayout.Space();
            fishType = (FishType) EditorGUILayout.ObjectField(fishType, typeof(FishType), false);
            EditorGUILayout.Space();
            if (fishType == null) 
                return;
            if (GUILayout.Button("Create With Of All Rarities"))
            {
                CreateAll();
            }
        }

        private void CreateAll()
        {
            var rarities = AssetQuerying.FindAssetsByType<Rarity>();
            foreach (var rarity in rarities)
            {
                var instance = CreateInstance<FishItem>();
                instance.rarity = rarity;
                instance.type = fishType;
                instance.name = $"{rarity.name}{fishType.name}";
                var directoryPath = $"{Folder}/{fishType.name}";
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                var fullPath = $"{directoryPath}/{rarity.name}{fishType.name}.asset";
                if (File.Exists(fullPath))
                    return;
                AssetDatabase.CreateAsset(instance, fullPath);
            }
        }
    }
}
