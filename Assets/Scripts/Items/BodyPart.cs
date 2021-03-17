using System;
using EventManagement;
using Events;
using Items.Gear;
using PlayerData;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class BodyPart : MonoBehaviour, IEquipmentSlot
    {
        [SerializeField] private string bodyPartName;
        [SerializeField] private EquipmentType myPreferredEquipment;        
        
        public IMessageHandler eventsBroker;
        public IEquippable EquippedItem { get; private set; }

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<CheckAndDoEquipEvent>(DoEquip);
        }

        public void DoEquip(CheckAndDoEquipEvent eventRef)
        {
            var itemToEquip = eventRef.Item;
            
                if (itemToEquip.Equipment.equipmentVariant.EquipmentType == myPreferredEquipment)
                {
                    if (EquippedItem == null)
                    {
                        UnEquipAndEquip(itemToEquip);
                        return;
                    }

                    if (itemToEquip.ID == EquippedItem.ID) return;
                    UnEquipAndEquip(itemToEquip);
                }
        }

        private void UnEquipAndEquip(GearInstance itemToEquip)
        {
            EquippedItem = null;
            EquippedItem = itemToEquip;
            Debug.Log($"{bodyPartName} equipped with {EquippedItem.Name}!");
        }
    }
}