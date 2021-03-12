using UnityEngine;

namespace Items
{
    public static class AllItems
    {
        private static ItemIndexer _itemIndexer;
        public static ItemIndexer ItemIndexer
        {
            get
            {
                if (_itemIndexer != null) 
                    return _itemIndexer;
                _itemIndexer = Resources.Load<ItemIndexer>("Global Item Index");
                #if UNITY_EDITOR
                if (_itemIndexer != null) 
                    return _itemIndexer;
                _itemIndexer = ScriptableObject.CreateInstance<ItemIndexer>();
                UnityEditor.AssetDatabase.CreateAsset(_itemIndexer, "Assets/Resources/Global Item Index.asset");
                #endif
                return _itemIndexer;
            }
        }
    }
}