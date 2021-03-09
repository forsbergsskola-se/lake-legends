using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Rarity")]
    public class Rarity : ScriptableObject
    {
        public int starAmount;
        public Color color;
        public Sprite sprite;
    }
}
