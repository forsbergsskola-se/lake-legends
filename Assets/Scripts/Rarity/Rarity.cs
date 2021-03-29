using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Rarity")]
    public class Rarity : ScriptableObject
    {
        public int starAmount;
        public int fusionCost = 100;
        public int sacrificeValue = 100;
        public Sprite frame;
        public Sprite background;
    }
}
