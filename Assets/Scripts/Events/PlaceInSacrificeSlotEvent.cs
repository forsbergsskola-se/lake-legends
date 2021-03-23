using PlayerData;

namespace Events
{
    internal class PlaceInSacrificeSlotEvent
    {
        public GearInstance gearInstance;

        public PlaceInSacrificeSlotEvent(GearInstance gearInstance) => this.gearInstance = gearInstance;
    }
}