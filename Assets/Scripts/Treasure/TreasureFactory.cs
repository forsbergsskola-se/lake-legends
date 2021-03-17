using System.Linq;
using Items;
using PlayerData;
using UnityEngine;

namespace Treasure
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TreasureFactory", fileName = "New TreasureFactory")]
    public class TreasureFactory : ScriptableObject, IItem, ICatchable
    {
        [SerializeField] private string itemID;
        public ScriptableObject[] treasureChests;
        public float[] weights;
        public FloatRange randomStopTimeRange = new FloatRange(0.5f, 2);
        public FloatRange randomMoveTimeRange = new FloatRange(1f, 3f);
        public float catchableSpeed = 1;
        public float catchableStrength = 1;
        public IItem GenerateTreasure()
        {
            Debug.Log("Generating Treasure");
            var randomNum = Random.Range(0f, weights.Sum());
            Debug.Log(randomNum);

            for (var i = 0; i < treasureChests.Length; i++)
            {
                if (randomNum < weights[i])
                {
                    return treasureChests[i] as IItem;
                }
                randomNum -= weights[i];
            }
            throw new System.Exception("TreasureFactory with name " + this.name + " Is Empty");
        }
        public void Use()
        {
            OpenTreasure();
        }

        void OpenTreasure()
        {
            var x = GenerateTreasure();
            Debug.Log("Generating treasure" + x.Name);
            FindObjectOfType<InventoryHandler>().AddItemToInventory(x);
        }
        
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(itemID)) itemID = System.Guid.NewGuid().ToString();
                    return itemID;
            }
        }
        public string Name => name;
        public int Rarity => 0;
        public FloatRange RandomStopTimeRange => randomStopTimeRange;
        public FloatRange RandomMoveTimeRange => randomMoveTimeRange;
        public float CatchableSpeed => catchableSpeed;
        public float CatchableStrength => catchableStrength;
    }
}