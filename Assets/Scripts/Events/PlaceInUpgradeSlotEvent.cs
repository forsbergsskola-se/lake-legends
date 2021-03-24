using PlayerData;

namespace Events
{
    public class PlaceInUpgradeSlotEvent
    {
        public GearInstance gearInstance;
        public PlaceInUpgradeSlotEvent(GearInstance gearInstance)
        {
            this.gearInstance = gearInstance;
        }
    }
}