using PlayerData;

namespace Events
{
    public class EnableFisherDexEvent
    {
        public readonly IInventory Inventory;

        public EnableFisherDexEvent(IInventory inventory)
        {
            Inventory = inventory;
        }
    }
}