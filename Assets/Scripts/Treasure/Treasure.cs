using System.Linq;
using Items;
using UnityEngine;

namespace Treasure
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TreasureFactory", fileName = "New TreasureFactory")]
    public class Factory : ScriptableObject, IItem
    {
        public string ID { get; }
        public string Name => name;
        public int Rarity => 0;

        IItem[] treasures;
        public float[] weights;

        public IItem GenerateTreasure()
        {
            var randomNum = Random.Range(0f, weights.Sum());
            Debug.Log(randomNum);

            for (var i = 0; i < treasures.Length; i++)
            {
                if (randomNum < weights[i])
                {
                    return treasures[i];
                }

                randomNum -= weights[i];
            }
            
            throw new System.Exception("TreasureFactory with name " + this.name + " Is Empty");
        }
    }
}