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
    public class GearInstance : IItem, IEquippable, ISellable
    {
        public GearSaveData GearSaveData;
        private Equipment equipment;
        private bool isEquipped;

        [JsonIgnore] public float CalculatedLineStrength => Mathf.Lerp(Equipment.lineStrength.Min, Equipment.lineStrength.Max, GearSaveData.lineStrength);
        [JsonIgnore] public float CalculatedAttraction => Mathf.Lerp(Equipment.attraction.Min, Equipment.attraction.Max, GearSaveData.attraction);
        [JsonIgnore] public float CalculatedAccuracy => Mathf.Lerp(Equipment.accuracy.Min, Equipment.accuracy.Max, GearSaveData.accuracy);

        [JsonIgnore] public Equipment Equipment
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
        [JsonIgnore] public EquipmentType EquipmentType => Equipment.EquipmentType;

        [JsonIgnore] public bool IsEquipped
        {
            get => isEquipped;
            set
            {
                if (value)
                {
                    Equipped?.Invoke();
                }
                else
                {
                    UnEquipped?.Invoke();
                }
                isEquipped = value;
            }
        }

        public event Action Equipped;
        public event Action UnEquipped;
        public event Action Sold;
        [JsonIgnore] public string Name => Equipment.equipmentVariant.name;
        [JsonIgnore] public int Rarity => Equipment.Rarity;
        public void Use()
        {
            
        }
        
        public void Equip()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            
            // Need to publish to showcase UI here in stead of event directly
            Debug.Log("Firing use-event");
            
            //TODO: Send event for opening a ViewItemInfoUI, that has a button that then fires CheckAndDoEquipEvent
            broker.Publish(new CheckAndDoEquipEvent(this));
            IsEquipped = true;
        }
        
        public void Sell()
        {
            Debug.Log("Sold");
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new RemoveItemFromInventoryEvent(this));
            broker.Publish(new UnEquipEvent(this));
            UnEquipped?.Invoke();
            Sold?.Invoke();
        }

        public override string ToString()
        {
            return
                $"Rarity: {Equipment.RarityName} \n" +
                $"Accuracy: {CalculatedAccuracy} \n" +
                $"Attraction: {CalculatedAttraction} \n" +
                $"Line Strength: {CalculatedLineStrength} \n" +
                $"Level: {GearSaveData.level}";
        }
    }
}
