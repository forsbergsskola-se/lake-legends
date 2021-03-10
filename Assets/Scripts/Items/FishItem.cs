using System;
using System.Collections;
using System.Collections.Generic;
using Fish;
using UnityEngine;

namespace Items
{    
    [CreateAssetMenu(menuName = "ScriptableObjects/FishItem")]
    public class FishItem : ScriptableObject, IItem
    {
        public int goldValue = 10;
        [SerializeField] private string ItemID;
        public FishType type;
        public Rarity rarity;
        public float rarityWeight = 100;
        public GameObject model;

        public float fishSpeed = 1;
        public float fishStrength = 1;

        public override string ToString()
        {
            return rarity.name + " " + type.name;
        }

        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(ItemID))
                    ItemID = Guid.NewGuid().ToString();
                return ItemID;
            }
        }
    }
}
