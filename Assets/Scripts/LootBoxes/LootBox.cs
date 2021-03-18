using System;
using System.Linq;
using EventManagement;
using Events;
using Items;
using Items.Gear;
using PlayerData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LootBoxes
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LootBox", fileName = "New LootBox")]
    public class LootBox : ScriptableObject, IItem, ICatchable, IOpenable
    {
        [SerializeField] private string itemID;
        public ScriptableObject[] loot;
        public float[] weights;
        
        public FloatRange randomStopTimeRange = new FloatRange(0.5f, 2);
        public FloatRange randomMoveTimeRange = new FloatRange(1f, 3f);
        public float catchableSpeed = 1;
        public float catchableStrength = 1;
        
        public IItem GenerateLoot()
        {
            Debug.Log("Generating Treasure");
            var randomNum = Random.Range(0f, weights.Sum());
            Debug.Log(randomNum);

            for (var i = 0; i < loot.Length; i++)
            {
                if (randomNum < weights[i])
                {
                    Debug.Log(loot[i].name);
                    return loot[i] as IItem;
                }
                randomNum -= weights[i];
            }
            throw new System.Exception("LootBox with name " + this.name + " Is Empty");
        }
        
        public void Use()
        {
            
        }

        void OpenLootBox()
        {
            var treasure = GenerateLoot();
            RemoveLootBox();
            if (treasure is Equipment equipment)
            {
                var gearInstance = new GearInstance(new GearSaveData(equipment));
                FindObjectOfType<EventsBroker>().Publish(new AddItemToInventoryEvent(gearInstance));
            }
            else
            {
                FindObjectOfType<EventsBroker>().Publish(new AddItemToInventoryEvent(treasure));
            }
            Debug.Log("Generating treasure" + treasure.Name);
        }

        void RemoveLootBox()
        {
            FindObjectOfType<EventsBroker>().Publish(new RemoveItemFromInventoryEvent(this));
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

        public override string ToString()
        {
            return name;
        }

        public void Open(Action openListener)
        {
            openListener?.Invoke();
            OpenLootBox();
        }
    }
}