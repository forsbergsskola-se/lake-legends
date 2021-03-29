using System;
using System.Linq;
using EventManagement;
using Events;
using Items;
using PlayerData;
using UnityEngine;

namespace UI
{
    public class FisherDexUI : InventoryUI
    {
        protected override void OnEnable()
        { 
            Clear();
            FindObjectOfType<EventsBroker>().SubscribeTo<EnableFisherDexEvent>(Setup);
            FindObjectOfType<EventsBroker>().Publish(new RequestFisherDexData());
        }
        
        private void Setup(EnableFisherDexEvent inventoryEvent)
        {
            Setup(inventoryEvent.Inventory);
        }
        
        protected override void Setup(IInventory inventory)
        {
            var items = inventory.GetAllItems();
            var allItems = AllItems.ItemIndexer.indexer;
            var allFishes = allItems.Where(fish => fish.Value is FishItem);
            foreach (var fish in allFishes)
            {
                var fishItem = fish.Value as FishItem;
                if (fishItem.type == null || fishItem.rarity == null)
                    continue;
                var instance = Instantiate(slotPrefab, gridParent);
                var hasCaught = items.ContainsKey(fish.Key);
                instance.Setup(fishItem, hasCaught);
                inventorySlots.Add(instance);
            }
            /*
            foreach (var item in items)
            {
                var instance = Instantiate(slotPrefab, gridParent);
                if (AllItems.ItemIndexer.indexer.ContainsKey(item.Key))
                    instance.Setup(AllItems.ItemIndexer.indexer[item.Key] as FishItem);
                inventorySlots.Add(instance);
            }*/
            SortAscended();
            /*for (var i = 0; i < AllItems.ItemIndexer.indexer.Count(item => item.Value is FishItem) - items.Count; i++)
            {
                var instance = Instantiate(slotPrefab, gridParent);
            }*/
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
                    .ThenBy(slot => ((FishItem) slot.Item).rarity.starAmount)
                    .ToArray();

            for (var i = 0; i < orderedList.Length; i++)
            {
                orderedList[i].transform.SetSiblingIndex(i);
            }
        }

        private void OnDisable()
        {
            FindObjectOfType<EventsBroker>()?.UnsubscribeFrom<EnableFisherDexEvent>(Setup);
        }
    }
}