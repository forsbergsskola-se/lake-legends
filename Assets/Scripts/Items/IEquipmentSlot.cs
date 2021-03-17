using Events;
using Items.Gear;

namespace Items
{
    public interface IEquipmentSlot
    {
        public IEquippable EquippedItem { get; }

        public void DoEquip(CheckAndDoEquipEvent eventRef);
    }
}
