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
        }
        
        private void Setup(EnableFisherDexEvent inventoryEvent)
        {
            Setup(inventoryEvent.Inventory);
        }
        
        protected override void Setup(IInventory inventory)
        {
            var items = inventory.GetAllItems();
            foreach (var pair in items)
            {
                Debug.Log(pair.Key);
            }
            foreach (var item in items)
            {
                var instance = Instantiate(slotPrefab, gridParent);
                if (AllItems.ItemIndexer.indexer.ContainsKey(item.Key))
                    instance.Setup(AllItems.ItemIndexer.indexer[item.Key] as FishItem);
                inventorySlots.Add(instance);
            }
            if (inventorySlots != null && inventorySlots.Count != 0)
                SortAscended();
            for (var i = 0; i < AllItems.ItemIndexer.indexer.Count(item => item.Value is FishItem) - items.Count; i++)
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