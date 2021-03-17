using System;
using EventManagement;
using Events;
using Items.Gear;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Items
{
    [Serializable]
    public class BodyPart : IEquipmentSlot
    {
        [SerializeField] private string bodyPartName;
        [SerializeField] private EquipmentType myPreferredEquipment;        
        
        public IMessageHandler eventsBroker;
        public IEquippable equippedItem { get; private set; }

        private void Start()
        {
            eventsBroker = Object.FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<CheckAndDoEquipEvent>(DoEquip);
        }

        public void DoEquip(CheckAndDoEquipEvent eventRef)
        {
            var itemToEquip = eventRef.Item;
            if (itemToEquip.EquipmentType == myPreferredEquipment)
            {
                if (equippedItem == null)
                {
                    UnEquipAndEquip(itemToEquip);
                    return;
                }
                if (itemToEquip.ID == equippedItem.ID) return;
                UnEquipAndEquip(itemToEquip);
            }
        }
  
        private void UnEquipAndEquip(IEquippable itemToEquip)
        {
            equippedItem = null;
            equippedItem = itemToEquip;
            Debug.Log($"Equipped {equippedItem.Name}!");
        }
    }
}