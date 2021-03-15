using Events;
using PlayerData;

namespace Items
{
    public class FisherDexUI : InventoryUI
    {
        protected override void Setup(IInventory inventory)
        {
            var items = inventory.GetAllItems();
            foreach (var item in items)
            {
                var instance = Instantiate(slotPrefab, gridParent);
                instance.Setup(AllItems.ItemIndexer.indexer[item.Key] as IItem);
                inventorySlots.Add(instance);
            }
            ToggleSort();
        }
    }
}