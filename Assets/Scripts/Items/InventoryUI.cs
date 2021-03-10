using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using PlayerData;
using UnityEngine;

namespace Items
{
    public class InventoryUI : MonoBehaviour
    {
        public InventorySlot slotPrefab;
        public Transform gridParent;
        public List<InventorySlot> inventorySlots;
        private bool decended;

        private void OnEnable()
        { 
            Clear();
            FindObjectOfType<EventsBroker>().SubscribeTo<EnableInventoryEvent>(Setup);
        }

        private void Clear()
        {
            foreach (var inventorySlot in inventorySlots)
            {
                Destroy(inventorySlot.gameObject);
            }
        }

        private void Setup(EnableInventoryEvent inventoryEvent)
        {
            Setup(inventoryEvent.Inventory);
        }

        private void Setup(IInventory inventory)
        {
            var items = inventory.GetAllItems();
            foreach (var item in items)
            {
                for (var i = 0; i < item.Value; i++)
                {
                    var instance = Instantiate(slotPrefab, gridParent);
                    instance.Setup(AllItems.ItemIndexer.indexer[item.Key] as IItem);
                    inventorySlots.Add(instance);
                }
            }
            ToggleSort();
            for (var i = inventorySlots.Count; i < inventory.MaxSize; i++)
            {
                var instance = Instantiate(slotPrefab, gridParent);
            }
        }

        public void ToggleSort()
        {
            if (decended)
                SortAscended();
            else
                SortDescended();
            decended = !decended;
        }

        public void SortDescended()
        {
            var nameSortedList = inventorySlots.OrderBy(slot => slot.Item.Name);
            var raritySortedList = nameSortedList.OrderByDescending(slot => slot.Item.Rarity).ToArray();
            for (var i = 0; i < raritySortedList.Length; i++)
            {
                raritySortedList[i].transform.SetSiblingIndex(i);
            }
        }
        
        public void SortAscended()
        {
            var nameSortedList = inventorySlots.OrderBy(slot => slot.Item.Name);
            var raritySortedList = nameSortedList.OrderBy(slot => slot.Item.Rarity).ToArray();
            for (var i = 0; i < raritySortedList.Length; i++)
            {
                raritySortedList[i].transform.SetSiblingIndex(i);
            }
        }
    }
}
