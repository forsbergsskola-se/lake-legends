using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ItemIndexer))]
    public class ItemIndexerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var itemIndexer = serializedObject.targetObject as ItemIndexer;
            if (GUILayout.Button("Populate List"))
            {
                PopulateItemIndexer(itemIndexer);
            }
        }

        private void PopulateItemIndexer(ItemIndexer itemIndexer)
        {
            var dictionary = new StringSoDictionary();
            foreach (var pairs in itemIndexer.indexer.Where(pairs => pairs.Value != null))
            {
                dictionary.Add(pairs.Key, pairs.Value);
            }

            itemIndexer.indexer = dictionary;
            var items = FindAllItems();
            foreach (var item in items)
            {
                if (!itemIndexer.indexer.ContainsKey(item.ID))
                    itemIndexer.indexer.Add(item.ID, item as ScriptableObject);
            }
        }
        
        public static IEnumerable<IItem> FindAllItems()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(ScriptableObject)}");
            var iItems = guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>).Where(asset => asset != null).OfType<IItem>();
            return iItems;
        }
    }
}