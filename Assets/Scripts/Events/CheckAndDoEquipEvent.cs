using PlayerData;

namespace Events
{
    public class CheckAndDoEquipEvent
    {
        public GearInstance Item;

        public CheckAndDoEquipEvent(GearInstance item) => this.Item = item;
    }
}