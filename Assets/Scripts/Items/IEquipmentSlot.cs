using Events;
using Items.Gear;

namespace Items
{
    public interface IEquipmentSlot
    {
        public IEquippable equippedItem { get; }

        public void DoEquip(CheckAndDoEquipEvent eventRef);
    }
}
