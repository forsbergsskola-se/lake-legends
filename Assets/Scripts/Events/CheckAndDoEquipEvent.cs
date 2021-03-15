using Items.Gear;

namespace Events
{
    public class CheckAndDoEquipEvent
    {
        public Equipment Item;

        public CheckAndDoEquipEvent(Equipment item) => this.Item = item;
    }
}