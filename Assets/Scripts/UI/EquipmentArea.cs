using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Items;
using PlayerData;
using UnityEngine;

namespace UI
{
    public class EquipmentArea : MonoBehaviour
    {
        public Slot slotPrefab;
        public Transform gridParent;
        public List<Slot> equipmentSlots = new List<Slot>();
        private IMessageHandler eventBroker;

        private void OnEnable()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker.SubscribeTo<UpdateEquipmentUI>(UpdateEquipment);
            eventBroker.Publish(new RequestEquipmentEvent());
        }

        private void UpdateEquipment(UpdateEquipmentUI eventRef)
        {
            Clear();
            Setup(eventRef.PlayerBody);
        }

        protected void Clear()
        {
            foreach (var child in transform.GetComponentsInChildren<Transform>())
            {
                if (child != transform)
                {
                    Destroy(child.gameObject);
                }
            }

            equipmentSlots = new List<Slot>();
        }
        
        protected void Setup(PlayerBody inventory)
        {
            var items = inventory.GetAllEquippedItems().Cast<IItem>();
            foreach (var item in items)
            {
                var instance = Instantiate(slotPrefab, gridParent);
                

                instance.Setup(item);
                

                equipmentSlots.Add(instance);
            }
  
            for (var i = equipmentSlots.Count; i < 6; i++)
            {
                var instance = Instantiate(slotPrefab, gridParent);
            }
        }

        private void OnDisable()
        {
            eventBroker?.UnsubscribeFrom<UpdateEquipmentUI>(UpdateEquipment);
        }
    }

    public class RequestEquipmentEvent
    {
        
    }

    public class UpdateEquipmentUI
    {
        public readonly PlayerBody PlayerBody;

        public UpdateEquipmentUI(PlayerBody playerBody)
        {
            this.PlayerBody = playerBody;
        }
    }
}