using PlayerData;

namespace Events
{
    public class PlaceInFuseSlotEvent
    {
        public GearInstance gearInstance;

        public PlaceInFuseSlotEvent(GearInstance gearInstance) => this.gearInstance = gearInstance;
    }
}