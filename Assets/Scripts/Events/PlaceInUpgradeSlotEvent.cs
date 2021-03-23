using PlayerData;

namespace Events
{
    public class PlaceInUpgradeSlotEvent
    {
        GearInstance gearInstance;
        public PlaceInUpgradeSlotEvent(GearInstance gearInstance)
        {
            this.gearInstance = gearInstance;
        }
    }
}