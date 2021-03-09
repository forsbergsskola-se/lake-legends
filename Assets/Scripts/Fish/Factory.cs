using System;
using System.Linq;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fish
{
    [CreateAssetMenu(menuName = "ScriptableObjects/FishFactory", fileName = "New FishFactory")]
    public class Factory : ScriptableObject
    {
        public FishItem[] fishItems;

        public FishItem GenerateFish()
        {
            var randomNum = Random.Range(0f, fishItems.Sum(item => item.rarityWeight));
            Debug.Log(randomNum);
            foreach (var t in fishItems)
            {
                if (randomNum < t.rarityWeight)
                {
                    return t;
                }
                randomNum -= t.rarityWeight;
            }
            throw new Exception("FishFactory With Name " + name + " Is Empty");
        }
    }
}
