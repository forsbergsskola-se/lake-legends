using System.Collections.Generic;
using System.Linq;
using Events;
using Fish;
using PlayerData;
using UnityEngine;

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
                instance.Setup(AllItems.ItemIndexer.indexer[item.Key] as FishItem);
                inventorySlots.Add(instance);
            }
            SortAscended();
            for (var i = 0; i < AllItems.ItemIndexer.indexer.Count - items.Count; i++)
            {
                var instance = Instantiate(slotPrefab, gridParent);
            }
        }

        public override void SortAscended()
        {
            var groupedSlots = inventorySlots
                .OrderBy(slot => ((FishItem) slot.Item).type.name);
            Sort(groupedSlots);
        }

        public override void SortDescended()
        {
            var groupedSlots = inventorySlots
                .OrderByDescending(slot => ((FishItem) slot.Item).type.name);
            Sort(groupedSlots);
        }

        private void Sort(IOrderedEnumerable<Slot> groupedSlots)
        {
            var orderedList = groupedSlots
                    .ThenByDescending(slot => ((FishItem) slot.Item).rarity.starAmount)
                    .ToArray();

            for (var i = 0; i < orderedList.Length; i++)
            {
                orderedList[i].transform.SetSiblingIndex(i);
            }
        }
    }
}