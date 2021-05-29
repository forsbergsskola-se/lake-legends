using System;
using Fish;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{    
    [CreateAssetMenu(menuName = "ScriptableObjects/FishItem")]
    public class FishItem : ScriptableObject, IItem, ICatchable
    {
        public bool givesGold;
        [FormerlySerializedAs("silverValue")] public int value = 10;
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
                throw new Exception("Item IDs Aren't Set Up Correctly!");
            }
        }

        public void GenerateNewGuid()
        {
            ItemID = Guid.NewGuid().ToString();
        }

        public string Name => name;
        public Sprite Sprite => type.sprite;
        public int RarityValue => rarity.starAmount;
        public Rarity Rarity => rarity;
        public FloatRange RandomStopTimeRange => randomStopTimeRange;
        public FloatRange RandomMoveTimeRange => randomMoveTimeRange;
        public float CatchableSpeed => fishSpeed;
        public float CatchableStrength => fishStrength;
        public string Bio => type.bio;
        public void Use()
        {
            
        }
    }
}