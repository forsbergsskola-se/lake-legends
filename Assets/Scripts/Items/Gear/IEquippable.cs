namespace Items.Gear
{
    public interface IEquippable
    {
        string Name { get; }
        string ID { get; }
        
        EquipmentType EquipmentType { get; }

        void Equip();
    }
}