using System;

namespace Items.Gear
{
    public interface IEquippable
    {
        event Action Equipped;

        event Action UnEquipped;

        string Name { get; }
        string ID { get; }
        
        EquipmentType EquipmentType { get; }
        
        bool IsEquipped { get; set; }

        void Equip();

        void Unequip();
    }
}