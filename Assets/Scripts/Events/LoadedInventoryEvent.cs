using PlayerData;
using Saving;

namespace Events
{
    public class LoadedInventoryEvent
    {
        public readonly ISaver Saver;
        public readonly IInventory Inventory;

        public LoadedInventoryEvent(ISaver saver, IInventory inventory)
        {
            Saver = saver;
            Inventory = inventory;
        }
    }
}