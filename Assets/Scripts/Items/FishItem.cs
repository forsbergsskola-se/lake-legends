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
        private string ItemID;
        public FishType type;
        public Rarity rarity;
        public float rarityWeight = 100;
        public GameObject model;

        public override string ToString()
        {
            return rarity.name + " " + type.name;
        }
    }
}
