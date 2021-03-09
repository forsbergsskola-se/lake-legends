using System;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Item Indexer")]
    public class ItemIndexer : ScriptableObject
    {
        public StringSoDictionary indexer;
    }
}

[Serializable]
public class StringSoDictionary : SerializableDictionary<string, ScriptableObject> {}