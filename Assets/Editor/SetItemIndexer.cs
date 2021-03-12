using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SetItemIndexer : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var itemIndexer = AllItems.ItemIndexer;
            if (itemIndexer == null)
            {
                itemIndexer = ScriptableObject.CreateInstance<ItemIndexer>();
                AssetDatabase.CreateAsset(itemIndexer, "Assets/Resources/Global Item Index.asset");
            }
            PopulateItemIndexer(itemIndexer);
        }
        
        private static void PopulateItemIndexer(ItemIndexer itemIndexer)
        {
            ClearDictionary(itemIndexer);
            var items = FindAllItems();
            foreach (var item in items)
            {
                if (!itemIndexer.indexer.ContainsKey(item.ID))
                    itemIndexer.indexer.Add(item.ID, item as ScriptableObject);
            }
            EditorUtility.SetDirty(itemIndexer);
        }

        private static void ClearDictionary(ItemIndexer itemIndexer)
        {
            itemIndexer.indexer.Clear();
        }
        
        public static IEnumerable<IItem> FindAllItems()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(ScriptableObject)}");
            var iItems = guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>).Where(asset => asset != null).OfType<IItem>();
            return iItems;
        }
    }
}