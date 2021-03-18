using PlayerData;

namespace Events
{
    public class CheckAndDoEquipEvent
    {
        public GearInstance Item;
        public bool Startup;

        public CheckAndDoEquipEvent(GearInstance item, bool startup = false)
        {
            Item = item;
            Startup = startup;
        }
    }
}