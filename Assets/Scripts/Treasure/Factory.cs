using System.Linq;
using Items;
using UnityEngine;

namespace Treasure
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TreasureFactory", fileName = "New TreasureFactory")]
    public class Factory : ScriptableObject, IItem
    {
        public ScriptableObject[] treasureChests;
        public float[] weights;

        public IItem GenerateTreasure()
        {
            var randomNum = Random.Range(0f, weights.Sum());
            Debug.Log(randomNum);

            for (var i = 0; i < treasureChests.Length; i++)
            {
                if (randomNum < weights[i])
                {
                    return (IItem)treasureChests[i];
                }

                randomNum -= weights[i];
            }
            
            throw new System.Exception("TreasureFactory with name " + this.name + " Is Empty");
        }

        public string ID { get; }
        public string Name => name;
        public int Rarity => 0;
    }
}