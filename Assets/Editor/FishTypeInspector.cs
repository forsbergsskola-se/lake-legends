using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fish;
using Items;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(FishType))]
    public class FishTypeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            OnGUI();
        }
        private const string Folder = "Assets/ScriptableObjects/Fish/Items";

        private void OnGUI()
        {
            var fishType = (FishType) serializedObject.targetObject;
            if (GUILayout.Button("Create With Of All Rarities"))
            {
                CreateAll(fishType);
            }
        }

        private void CreateAll(FishType fishType)
        {
            var rarities = FindAssetsByType<Rarity>();
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
        
        public static IEnumerable<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            return guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).Where(asset => asset != null).ToList();
        }
    }
}