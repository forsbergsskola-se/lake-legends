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
        [SerializeField] private string itemID;
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
        
        public string ID
        {
            get
            
            {
                if (string.IsNullOrEmpty(itemID)) itemID = System.Guid.NewGuid().ToString();
                    return itemID;
                
                Debug.LogError("Item IDs Aren't Set Up Correctly!");
                throw new System.Exception("Item IDs Aren't Set Up Correctly!");
            }
        }

        
        public string Name => name;
        public int Rarity => 0;
    }
}