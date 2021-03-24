using PlayerData;

namespace Events
{
    public class PlaceInSacrificeSlotEvent
    {
        public GearInstance gearInstance;

        public PlaceInSacrificeSlotEvent(GearInstance gearInstance) => this.gearInstance = gearInstance;
    }
}