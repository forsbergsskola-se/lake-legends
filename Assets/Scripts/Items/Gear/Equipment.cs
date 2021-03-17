using System;
using UnityEngine;

namespace Items.Gear
{
    [CreateAssetMenu(fileName = "New Equipment", menuName =  "ScriptableObjects/Equipment")]
    public class Equipment : ScriptableObject, IItem
    {
        [SerializeField] private string ItemID;

        [SerializeField] public EquipmentVariant equipmentVariant;
        [SerializeField] private Rarity rarity;
        
        [SerializeField] public FloatRange lineStrength = new FloatRange(22,28);
        [SerializeField] public FloatRange attraction = new FloatRange(22,28);
        [SerializeField] public FloatRange accuracy = new FloatRange(22,28);
        
        public string ID {
            get
            {
                if (!string.IsNullOrEmpty(ItemID)) return ItemID;
                Debug.LogError("Item IDs Aren't Set Up Correctly!");
                throw new Exception("Item IDs Aren't Set Up Correctly!");
            }
        }

        public EquipmentType EquipmentType => equipmentVariant.EquipmentType;

        public string Name => name;
        public int Rarity => rarity.starAmount;
        
        public override string ToString()
        {
            return rarity.name + " " + this.name;
        }
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                ItemID = Guid.NewGuid().ToString();
            }
        }
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                ItemID = Guid.NewGuid().ToString();
            }
            lineStrength.Validate();
            attraction.Validate();
            accuracy.Validate();
        }
        
        public void Use()
        {

        }
    }
}