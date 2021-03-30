using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using Items;
using Items.Gear;
using Saving;
using UI;
using UnityEngine;

namespace PlayerData
{
    public class PlayerBody : MonoBehaviour
    {
        [SerializeField] private BodyPart[] bodyParts;

        private const string SaveKey = "Equipment";
        
        private float totalLineStrength;
        private float totalAttraction;
        private float totalAccuracy;

        public IMessageHandler eventsBroker;
        private EquipmentSaver equipmentSaver;
        
        public float TotalLineStrength
        {
            get
            {
                totalLineStrength = 0f;
                foreach (var bodyPart in bodyParts)
                {
                    if(bodyPart.EquippedItem is GearInstance reference) 
                        totalLineStrength += reference.CalculatedLineStrength;
                }

                return totalLineStrength;
            }   
        }

        public float TotalAttraction
        {
            get
            {
                totalAttraction = 0f;
                foreach (var bodyPart in bodyParts)
                {
                    if(bodyPart.EquippedItem is GearInstance reference) 
                        totalAttraction += reference.CalculatedAttraction;
                }

                return totalAttraction;
            }   
        }
        
        public float TotalAccuracy
        {
            get
            {
                totalAccuracy = 0f;
                foreach (var bodyPart in bodyParts)
                {
                    if(bodyPart.EquippedItem is GearInstance reference) 
                        totalAccuracy += reference.CalculatedAccuracy;
                }
                return totalAccuracy;
            }   
        }

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            foreach (var bodypart in bodyParts)
            {
                bodypart.WakeUp(eventsBroker, this);
            }
            
            eventsBroker.SubscribeTo<LoadedInventoryEvent>(Initialize);
            eventsBroker.SubscribeTo<RequestEquipmentEvent>(eventRef => eventsBroker.Publish(new UpdateEquipmentUI(this)));
        }
        
        async void Initialize(LoadedInventoryEvent eventRef)
        {
            var saver = eventRef.Saver;

            equipmentSaver = new EquipmentSaver(saver, new JsonSerializer());
            var loadedEquipment = await equipmentSaver.LoadEquipment(SaveKey);
            
            if (loadedEquipment == null || loadedEquipment.Length == 0) 
                return;

            var allGear = eventRef.Inventory.GetGear();
            if (allGear == null || allGear.Count == 0)
                return;
            foreach (var t in loadedEquipment)
            {
                var item = allGear[t];
                eventsBroker.Publish(new CheckAndDoEquipEvent(item, true));
            }
        }

        public IEnumerable<IEquippable> GetAllEquippedItems()
        {
            return bodyParts.Where(part => part.EquippedItem != null).Select(part => part.EquippedItem);
        }
        
        public void SaveEquipment()
        {
            var idArray = bodyParts.Where(part => part.EquippedItem != null).Select(part => part.EquippedItem.ID).ToArray();
            
            equipmentSaver.SaveEquipment(SaveKey, idArray);
        }
    }
}