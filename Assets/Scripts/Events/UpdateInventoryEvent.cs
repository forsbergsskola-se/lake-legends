using Items;
using PlayerData;

namespace Events
{
    public class UpdateInventoryEvent
    {
        public readonly bool Added;
        public readonly IItem Item;
        public readonly GearInventory GearInventory;

        public UpdateInventoryEvent(bool added, IItem item, GearInventory gearInventory)
        {
            this.Added = added;
            Item = item;
            GearInventory = gearInventory;
        }
    }
}