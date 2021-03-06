using System;
using System.Linq;
using Items;
using LootBoxes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fish
{
    [CreateAssetMenu(menuName = "ScriptableObjects/FishFactory", fileName = "New FishFactory")]
    public class Factory : ScriptableObject
    {
        public FishItem[] fishItems;
        public LootBox treasureChest;
        public float treasureChestWeight;

        public ICatchable GenerateFish(float attractionValue)
        {
            var multiplier = attractionValue * 0.001f + 1;
            var commonSum = fishItems.Where(fishItem => fishItem.RarityValue == 0).Sum(fishItem => fishItem.rarityWeight);
            var unCommonSum = fishItems.Where(fishItem => fishItem.RarityValue != 0).Sum(fishItem => fishItem.rarityWeight * multiplier);
            
            var randomNum = Random.Range(0f, commonSum + unCommonSum + treasureChestWeight);
            
            foreach (var t in fishItems)
            {
                if (t.RarityValue != 0)
                {
                    if (randomNum < t.rarityWeight * multiplier)
                    {
                        return t;
                    }
                }
                else if (randomNum < t.rarityWeight)
                {
                    return t;
                }
                randomNum -= t.rarityWeight;
            }
            if(treasureChest != null)
                return treasureChest;
            throw new Exception("FishFactory With Name " + name + " Is Empty");
        }
    }
}