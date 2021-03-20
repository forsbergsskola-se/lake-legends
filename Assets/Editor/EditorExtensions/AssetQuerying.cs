using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace EditorExtensions
{
    public static class AssetQuerying
    {
        public static IEnumerable<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            return guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).Where(asset => asset != null);
        }
        
        public static IEnumerable<string> FindPathsForAllAssetsWithFileExtension(string extension)
        {
            var guids = AssetDatabase.FindAssets("");
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            return paths.Where(path => path.EndsWith(extension));
        }
        
        public static T FindAssetAtPath<T>(string path) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}