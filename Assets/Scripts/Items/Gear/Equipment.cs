using System;
using EventManagement;
using Events;
using UnityEngine;

namespace Items.Gear
{
    [CreateAssetMenu(fileName = "New Equipment", menuName =  "ScriptableObjects/Equipment")]
    public class Equipment : ScriptableObject, IItem, IEquippable
    {
        [SerializeField] private string ItemID;
        
        [SerializeField] public EquipmentType equipmentType;
        [SerializeField] private Rarity rarity;
        
        public string ID {
            get
            {
                if (!string.IsNullOrEmpty(ItemID)) return ItemID;
                Debug.LogError("Item IDs Aren't Set Up Correctly!");
                throw new Exception("Item IDs Aren't Set Up Correctly!");
            }
        }

        public string Name => name;
        public int Rarity { get; }
        
        public override string ToString()
        {
            return rarity.name + " " + this.name;
        }
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                ItemID = Guid.NewGuid().ToString();
            }
        }
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                ItemID = Guid.NewGuid().ToString();
            }
        }
        
        public void Use()
        {
            var broker = FindObjectOfType<EventsBroker>();
            
            // Need to publish to showcase UI here in stead of event directly
            Debug.Log("Firing use-event");
            broker.Publish(new CheckAndDoEquipEvent(this));
        }
    }
}