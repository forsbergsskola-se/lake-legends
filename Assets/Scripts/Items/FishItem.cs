using System;
using Fish;
using Player;
using UnityEngine;

namespace Items
{    
    [CreateAssetMenu(menuName = "ScriptableObjects/FishItem")]
    public class FishItem : ScriptableObject, IItem
    {
        public int silverValue = 10;
        [SerializeField] private string ItemID;
        public FishType type;
        public Rarity rarity;
        public float rarityWeight = 100;
        public GameObject model;
        public FloatRange randomStopTimeRange = new FloatRange(0.5f, 2);
        public FloatRange randomMoveTimeRange = new FloatRange(1f, 3f);

        public float fishSpeed = 1;
        public float fishStrength = 1;

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
            randomMoveTimeRange.Validate();
            randomStopTimeRange.Validate();
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
        public int Rarity => rarity.starAmount;
    }
}
