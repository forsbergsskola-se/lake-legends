using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Rarity")]
    public class Rarity : ScriptableObject
    {
        public int starAmount;
        public int fusionCost = 100;
        public int sacrificeValue = 100;
        public Color color;
        public Sprite sprite;
    }
}
