using PlayerData;

namespace Events
{
    public class PlaceInFusionUpgradeSlotEvent
    {
        public GearInstance gearInstance;
        public PlaceInFusionUpgradeSlotEvent(GearInstance gearInstance)
        {
            this.gearInstance = gearInstance;
        }
    }
}