using System;
using EventManagement;
using Events;
using Items;
using Items.Gear;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerData
{
    public class GearInstance : IItem, IEquippable
    {
        public GearSaveData GearSaveData;
        private Equipment equipment;

        [JsonIgnore] public float CalculatedLineStrength => Mathf.Lerp(Equipment.lineStrength.Min, Equipment.lineStrength.Max, GearSaveData.lineStrength);
        [JsonIgnore] public float CalculatedAttraction => Mathf.Lerp(Equipment.attraction.Min, Equipment.attraction.Max, GearSaveData.attraction);
        [JsonIgnore] public float CalculatedAccuracy => Mathf.Lerp(Equipment.accuracy.Min, Equipment.accuracy.Max, GearSaveData.accuracy);

        public Equipment Equipment
        {
            get
            {
                if (equipment == null) 
                    equipment = AllItems.ItemIndexer.indexer[GearSaveData.equipID] as Equipment;
                return equipment;
            }
        }

        public GearInstance(GearSaveData gearSaveData)
        {
            GearSaveData = gearSaveData;
        }

        [JsonConstructor]
        public GearInstance()
        {
            
        }

        public string ID => GearSaveData.instanceID;
        [JsonIgnore] public EquipmentType EquipmentType => Equipment.equipmentVariant.EquipmentType;
        [JsonIgnore] public string Name => Equipment.Name;
        [JsonIgnore] public int Rarity => Equipment.Rarity;
        public void Use()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            
            // Need to publish to showcase UI here in stead of event directly
            Debug.Log("Firing use-event");
            
            //TODO: Send event for opening a ViewItemInfoUI, that has a button that then fires CheckAndDoEquipEvent
            broker.Publish(new CheckAndDoEquipEvent(this));
        }
    }
}
