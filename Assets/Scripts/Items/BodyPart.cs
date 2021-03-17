using System;
using EventManagement;
using Events;
using Items.Gear;
using PlayerData;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class BodyPart : IEquipmentSlot
    {
        [SerializeField] private string bodyPartName;
        [SerializeField] private EquipmentType myPreferredEquipment;

        private PlayerBody playerBody;
        public IMessageHandler eventsBroker;
        public IEquippable EquippedItem { get; private set; }

        public void WakeUp(IMessageHandler messageHandler, PlayerBody playerBody)
        {
            eventsBroker = messageHandler;
            this.playerBody = playerBody;
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
            var havePreviousEquipment = EquippedItem != null;
            EquippedItem = null;
            EquippedItem = itemToEquip;

            if(havePreviousEquipment)
                playerBody.SaveEquipment();
            
            Debug.Log($"{bodyPartName} equipped with {EquippedItem.Name}!");
        }
    }
}