using PlayerData;

namespace Events
{
    public class EnableInventoryEvent
    {
        public readonly IInventory Inventory;

        public EnableInventoryEvent(IInventory inventory)
        {
            this.Inventory = inventory;
        }
    }
}