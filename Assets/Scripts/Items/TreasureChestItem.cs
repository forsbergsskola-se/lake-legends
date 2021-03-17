using System;
using Fish;
using Items.Gear;
using Player;
using UnityEngine;

namespace Items
{    
    [CreateAssetMenu(menuName = "ScriptableObjects/TreasureChestItem")]
    public class TreasureChestItem : ScriptableObject, IItem
    {
        [SerializeField] Equipment[] content;
        [SerializeField] int silverAmount;
        
        [SerializeField] private string ItemID;
        FishType type;
        Rarity rarity;
        public GameObject model;

        public override string ToString()
        {
            return rarity.name + " " + type.name;
        }
        private void Awake()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                ItemID = Guid.NewGuid().ToString();
            }
        }
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                ItemID = Guid.NewGuid().ToString();
            }
        }

        public string ID
        {
            get
            {
                if (!string.IsNullOrEmpty(ItemID)) return ItemID;
                Debug.LogError("Item IDs Aren't Set Up Correctly!");
                throw new Exception("Item IDs Aren't Set Up Correctly!");
            }
        }

        public string Name => name;
        public int Rarity => 0;
        public void Use()
        {
            Debug.Log($"Use {Name}");
        }
    }
}