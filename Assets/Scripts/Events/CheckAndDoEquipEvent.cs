using Items.Gear;

namespace Events
{
    public class CheckAndDoEquipEvent
    {
        public IEquippable Item;

        public CheckAndDoEquipEvent(IEquippable item) => this.Item = item;
    }
}