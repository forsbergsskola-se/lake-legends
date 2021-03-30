using System;
using System.Linq;
using EventManagement;
using Events;
using Items;
using Items.Gear;
using PlayerData;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LootBoxes
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LootBox", fileName = "New LootBox")]
    public class LootBox : ScriptableObject, IItem, ICatchable, IOpenable
    {
        [SerializeField] private string itemID;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Rarity rarity;
        public ScriptableObject[] loot;
        public float[] weights;
        
        public FloatRange randomStopTimeRange = new FloatRange(0.5f, 2);
        public FloatRange randomMoveTimeRange = new FloatRange(1f, 3f);
        public float catchableSpeed = 1;
        public float catchableStrength = 1;

        public bool isLootStealer = false;
        
        public IItem GenerateLoot()
        {
            if(weights.Length == 0)
            {
                var i = Random.Range(0, loot.Length);

                if (isLootStealer && loot[i] is LootBox lootBox)
                {
                    return lootBox.GenerateLoot();
                }
                return loot[i] as IItem;
            }
            var randomNum = Random.Range(0f, weights.Sum());

            for (var i = 0; i < loot.Length; i++)
            {
                if (randomNum < weights[i])
                {
                    if (isLootStealer && loot[i] is LootBox lootBox)
                    {
                        return lootBox.GenerateLoot();
                    }
                    return loot[i] as IItem;
                }
                randomNum -= weights[i];
            }
            throw new System.Exception("LootBox with name " + this.name + " Is Empty");
        }
        
        public void GenerateNewGuid()
        {
            itemID = Guid.NewGuid().ToString();
        }

        public Rarity Rarity => rarity;

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
            FindObjectOfType<ChestOpening>(true).StartOpening(this);
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
        public Sprite Sprite => sprite;
        public int RarityValue => 0;
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