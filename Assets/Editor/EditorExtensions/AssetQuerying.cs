using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public static class AssetQuerying
    {
        public static IEnumerable<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            return guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).Where(asset => asset != null);
        }
        
        public static IEnumerable<string> FindPathsForAllScenes(string sceneFolder)
        {
            var fullPath = Path.Combine(Application.dataPath, sceneFolder);
            var allFiles = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".unity"));
            var stringSeparator = Application.dataPath + "/";
            var relativePaths = allFiles.Select(s => s.Remove(0, stringSeparator.Length));
            return relativePaths;
        }
        
        public static IEnumerable<string> FindPathsForAllAssetsWithFileExtension(string extension)
        {
            var guids = AssetDatabase.FindAssets("");
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            return paths.Where(path => path.EndsWith(extension));
        }

        public static void DeleteFileAtRelativePath(string path)
        {
            var fullPath = Path.Combine("Assets", path);
            AssetDatabase.DeleteAsset(fullPath);
        }
        
        public static T FindAssetAtPath<T>(string path) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>("Assets/"+path);
        }
    }
}