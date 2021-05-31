using System;
using System.Linq;
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

        [JsonIgnore] public float CalculatedLineStrength => Mathf.Lerp(Equipment.lineStrength.Min, Equipment.lineStrength.Max, GearSaveData.lineStrength) + GearSaveData.GearLevel.Level * Equipment.lineStrengthIncreasePerLevel;
        [JsonIgnore] public float CalculatedAttraction => Mathf.Lerp(Equipment.attraction.Min, Equipment.attraction.Max, GearSaveData.attraction) + GearSaveData.GearLevel.Level * Equipment.attractionIncreasePerLevel;
        [JsonIgnore] public float CalculatedAccuracy => Mathf.Lerp(Equipment.accuracy.Min, Equipment.accuracy.Max, GearSaveData.accuracy) + GearSaveData.GearLevel.Level * Equipment.accuracyIncreasePerLevel;

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
                isEquipped = value;
                if (isEquipped)
                {
                    Equipped?.Invoke();
                }
                else
                {
                    UnEquipped?.Invoke();
                }
            }
        }

        public event Action Equipped;
        public event Action UnEquipped;
        public event Action Sold;

        public int GetSacrificeValue()
        {
            return Equipment.RaritySacrificeValue;
        }
        public int GetFusionCost()
        {
            return Equipment.FusionCost;
        }
        public void GenerateNewGuid()
        {
            GearSaveData.instanceID = Guid.NewGuid().ToString();
        }

        
        
        //TODO: Seperate Name and Level Text
        [JsonIgnore] public string Name => Equipment.Name + "\nLvl " + Level;
        [JsonIgnore] public int Level => GearSaveData.GearLevel.Level;
        [JsonIgnore] public int RarityValue => Equipment.RarityValue;
        [JsonIgnore] public Rarity Rarity => Equipment.Rarity;
        [JsonIgnore] public Sprite Sprite => Equipment.Sprite;

        public void UpgradeRarity()
        {
            var itemsOfSameType = AllItems.ItemIndexer.indexer.Values
                .Where(item => item is Equipment)
                .Cast<Equipment>()
                .Where(gear => gear.equipmentType == EquipmentType && gear.equipmentVariant == Equipment.equipmentVariant);
            var broker = Object.FindObjectOfType<EventsBroker>();
            DestroyItem();
            var gearSaveData = GearSaveData;
            var upgradedItem = itemsOfSameType.Where(gear => gear.RarityValue > RarityValue).OrderBy(gear => gear.RarityValue).First();
            gearSaveData.GearLevel.Level = 1;
            gearSaveData.equipID = upgradedItem.ID;
            broker.Publish(new AddItemToInventoryEvent(new GearInstance(gearSaveData)));
        }
        public void Use()
        {
            
        }

        public GearLevel IncreaseExp(int amount)
        {
            return GearSaveData.GearLevel = GearSaveData.GearLevel.IncreaseExp(amount, Rarity);
        }

        public GearLevel GetLevelInfoAfterIncrease(int amount) => GearSaveData.GearLevel.IncreaseExp(amount, Rarity);
        
        public void Equip()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new CheckAndDoEquipEvent(this));
            IsEquipped = true;
        }

        public void Unequip()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new UnEquipEvent(this));
            IsEquipped = false;
        }

        public void AddToFuseSlotArea()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new PlaceInFuseSlotEvent(this));
        }

        public void AddToSacrificeArea()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new PlaceInSacrificeSlotEvent(this));
        }
        
        public void Sacrifice()
        {
            DestroyItem();
        }

        public bool OpenFusionArea()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new PlaceInFusionUpgradeSlotEvent(this));
            return true;
        }
        
        public bool OpenUpgradeArea()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new PlaceInUpgradeSlotEvent(this));
            return true;
        }

        private void DestroyItem()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new RemoveItemFromInventoryEvent(this));
            UnEquipped?.Invoke();
            broker.Publish(new UnEquipEvent(this));
            Sold?.Invoke();
        }
        
        public void Sell()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new IncreaseSilverEvent(Equipment.SilverValue));
            broker.Publish(new PlaySoundEvent(SoundType.Sfx, "SellItemSound"));
            DestroyItem();
        }

        public int Value => Equipment.SilverValue;

        public override string ToString()
        {
            return
                $"Rarity: {Equipment.RarityName} \n" +
                $"Accuracy: {CalculatedAccuracy} \n" +
                $"Attraction: {CalculatedAttraction} \n" +
                $"Line Strength: {CalculatedLineStrength} \n" +
                $"Level: {GearSaveData.GearLevel.Level}";
        }

        public string[] GetStats()
        {
            return new[]
            {
                $"Accuracy: {CalculatedAccuracy:F0}",
                $"Attraction: {CalculatedAttraction:F0}",
                $"Line Strength: {CalculatedLineStrength:F0}"
            };
        }
    }
}
