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
            eventsBroker.SubscribeTo<UnEquipEvent>(UnEquip);
        }

        private void UnEquip(UnEquipEvent eventRef)
        {
            var itemToEquip = eventRef.Equippable;
            
            if (itemToEquip.IsEquipped && itemToEquip.Equipment.EquipmentType == myPreferredEquipment)
            {
                EquippedItem = null;
                playerBody.SaveEquipment();
            }
        }

        public void DoEquip(CheckAndDoEquipEvent eventRef)
        {
            var itemToEquip = eventRef.Item;
            
                if (itemToEquip.Equipment.EquipmentType == myPreferredEquipment)
                {
                    if (EquippedItem == null)
                    {
                        UnEquipAndEquip(itemToEquip, eventRef.Startup);
                        return;
                    }

                    if (itemToEquip.ID == EquippedItem.ID) return;
                    UnEquipAndEquip(itemToEquip, eventRef.Startup);
                }
        }

        private void UnEquipAndEquip(GearInstance itemToEquip, bool startUp)
        {
            if (EquippedItem != null)
                EquippedItem.IsEquipped = false;
            EquippedItem = null;
            EquippedItem = itemToEquip;
            if (startUp)
                EquippedItem.IsEquipped = true;

            if (!startUp)
                playerBody.SaveEquipment();
        }
    }
}