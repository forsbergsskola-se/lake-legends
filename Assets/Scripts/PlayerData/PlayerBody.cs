using System.Linq;
using EventManagement;
using Events;
using Items;
using Saving;
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
            
            eventsBroker.SubscribeTo<LoginEvent>(Initialize);
        }
        
        async void Initialize(LoginEvent eventRef)
        {
            ISaver saver;
            
            if (eventRef.Debug)
            {
                saver = new PlayerPrefsSaver();
            }
            else
            {
                saver = new DataBaseSaver(eventRef.User);
            }
            
            equipmentSaver = new EquipmentSaver(saver, new JsonSerializer());
            var loadedEquipment = await equipmentSaver.LoadEquipment(SaveKey);

            if (loadedEquipment == null || loadedEquipment.Length == 0) 
                return;
            
            var allGear = FindObjectOfType<InventoryHandler>().CurrentInventory.GetGear();
            
            for (int i = 0; i < allGear.Count; i++)
            {
                var item = allGear[loadedEquipment[i]];
                eventsBroker.Publish(new CheckAndDoEquipEvent(item));    
            }
        }
        
        public void SaveEquipment()
        {
            var idArray = bodyParts.Where(part => part.EquippedItem != null).Select(part => part.EquippedItem.ID).ToArray();
            
            equipmentSaver.SaveEquipment(SaveKey, idArray);
        }
    }
}