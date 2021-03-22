using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Items;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [InitializeOnLoad]
    public class SetItemIndexer : AssetPostprocessor
    {
        static SetItemIndexer()
        {
            CheckDependenciesAtStart();
        }

        private static async void CheckDependenciesAtStart()
        {
            await Task.Delay(1);
            CheckAndFixDependencies();
        }
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            CheckAndFixDependencies();
        }

        private static void CheckAndFixDependencies()
        {
            var itemIndexer = AllItems.ItemIndexer;
            if (itemIndexer == null)
            {
                itemIndexer = ScriptableObject.CreateInstance<ItemIndexer>();
                AssetDatabase.CreateAsset(itemIndexer, "Assets/Resources/Global Item Index.asset");
            }
            CheckAndFixItemIndexer(itemIndexer);
        }
        
        private static void CheckAndFixItemIndexer(ItemIndexer itemIndexer)
        {
            var items = FindAllItems();
            var enumerable = items.ToList();
            if (enumerable.All(item => itemIndexer.indexer.ContainsKey(item.ID)))
                return;

            ClearDictionary(itemIndexer);
            foreach (var item in enumerable)
            {
                /*if (itemIndexer.indexer.ContainsKey(item.ID))
                    item.GenerateNewGuid();
                EditorUtility.SetDirty(item as ScriptableObject);*/
                itemIndexer.indexer.Add(item.ID, item as ScriptableObject);
            }

            EditorUtility.SetDirty(itemIndexer);
        }

        private static void ClearDictionary(ItemIndexer itemIndexer)
        {
            itemIndexer.indexer.Clear();
        }

        private static IEnumerable<IItem> FindAllItems()
        {
            return AssetQuerying.FindAssetsByType<ScriptableObject>().OfType<IItem>();
        }
    }
}