using EventManagement;
using Events;
using Items;
using Items.Gear;
using UnityEngine;

namespace PlayerData
{
    public class GearInstance : IItem, IEquippable
    {
        private GearSaveData gearSaveData;
        private Equipment equipment;

        private string instanceID;

        private float CalculatedLineStrength => Mathf.Lerp(Equipment.lineStrength.Min, Equipment.lineStrength.Max, gearSaveData.LineStrength);
        private float CalculatedAttraction => Mathf.Lerp(Equipment.attraction.Min, Equipment.attraction.Max, gearSaveData.Attraction);
        private float CalculatedAccuracy => Mathf.Lerp(Equipment.accuracy.Min, Equipment.accuracy.Max, gearSaveData.Accuracy);

        private Equipment Equipment
        {
            get
            {
                if (equipment == null) equipment = AllItems.ItemIndexer.indexer[gearSaveData.equipID] as Equipment;
                return equipment;
            }
        }

        public GearInstance(GearSaveData gearSaveData)
        {
            this.gearSaveData = gearSaveData;
        }

        public string ID => instanceID;
        public EquipmentType EquipmentType => Equipment.equipmentVariant.EquipmentType;
        public string Name => Equipment.Name;
        public int Rarity => Equipment.Rarity;
        public void Use()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            
            // Need to publish to showcase UI here in stead of event directly
            Debug.Log("Firing use-event");
            broker.Publish(new CheckAndDoEquipEvent(this));
        }
    }
}
