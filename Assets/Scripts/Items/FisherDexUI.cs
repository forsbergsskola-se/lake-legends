using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using Fish;
using PlayerData;
using UnityEngine;

namespace Items
{
    public class FisherDexUI : InventoryUI
    {
        protected override void OnEnable()
        { 
            Clear();
            FindObjectOfType<EventsBroker>().SubscribeTo<EnableFisherDexEvent>(Setup);
        }
        
        protected override void Setup(EnableInventoryEvent inventoryEvent)
        {
            
        }

        private void Setup(EnableFisherDexEvent inventoryEvent)
        {
            Setup(inventoryEvent.Inventory);
        }
        
        protected override void Setup(IInventory inventory)
        {
            var items = inventory.GetAllItems();
            foreach (var item in items)
            {
                var instance = Instantiate(slotPrefab, gridParent);
                if (AllItems.ItemIndexer.indexer.ContainsKey(item.Key))
                    instance.Setup(AllItems.ItemIndexer.indexer[item.Key] as FishItem);
                inventorySlots.Add(instance);
            }
            if (inventorySlots != null && inventorySlots.Count != 0)
                ToggleSort();
            for (var i = 0; i < AllItems.ItemIndexer.indexer.Count - items.Count; i++)
            {
                var instance = Instantiate(slotPrefab, gridParent);
            }
        }

        public override void SortAscended()
        {
            var groupedSlots = inventorySlots.GroupBy(slot => (slot.Item as FishItem).type).OrderBy(slots => slots.Key.name);
            Sort(groupedSlots);
        }

        public override void SortDescended()
        {
            var groupedSlots = inventorySlots.GroupBy(slot => (slot.Item as FishItem).type).OrderByDescending(slots => slots.Key.name);
            Sort(groupedSlots);
        }

        private void Sort(IOrderedEnumerable<IGrouping<FishType, Slot>> groupedSlots)
        {
            var orderedList = groupedSlots.Select(groups => groups.OrderByDescending(slot => (slot.Item as FishItem).rarity.starAmount));
            var slotsOrdered = orderedList.SelectMany(order => order).ToArray();

            for (var i = 0; i < slotsOrdered.Length; i++)
            {
                slotsOrdered[i].transform.SetSiblingIndex(i);
            }
        }
    }
}