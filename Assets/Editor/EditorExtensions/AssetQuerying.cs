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
    }
}