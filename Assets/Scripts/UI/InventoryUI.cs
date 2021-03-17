using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using Items;
using PlayerData;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        public Slot slotPrefab;
        public Transform gridParent;
        public List<Slot> inventorySlots;
        private bool decended;

        protected virtual void OnEnable()
        { 
            Clear();
            FindObjectOfType<EventsBroker>().SubscribeTo<EnableInventoryEvent>(Setup);
        }

        protected virtual void Clear()
        {
            foreach (var inventorySlot in inventorySlots)
            {
                Destroy(inventorySlot.gameObject);
            }
        }

        protected virtual void Setup(EnableInventoryEvent inventoryEvent)
        {
            Setup(inventoryEvent.Inventory);
        }

        protected virtual void Setup(IInventory inventory)
        {
            var items = inventory.GetAllItems();
            foreach (var item in items)
            {
                for (var i = 0; i < item.Value; i++)
                {
                    var instance = Instantiate(slotPrefab, gridParent);
                    if (AllItems.ItemIndexer.indexer.ContainsKey(item.Key))
                        instance.Setup(AllItems.ItemIndexer.indexer[item.Key] as IItem);
                    else
                    {
                        var gearItem = inventory.GetGear()[item.Key];
                        instance.Setup(gearItem);
                    }
                    inventorySlots.Add(instance);
                }
            }
            ToggleSort();
            for (var i = inventorySlots.Count; i < inventory.MaxSize; i++)
            {
                var instance = Instantiate(slotPrefab, gridParent);
            }
        }

        public virtual void ToggleSort()
        {
            if (decended)
                SortAscended();
            else
                SortDescended();
            decended = !decended;
        }

        public virtual void SortDescended()
        {
            var nameSortedList = inventorySlots.OrderBy(slot => slot.Item.Name);
            var raritySortedList = nameSortedList.OrderByDescending(slot => slot.Item.Rarity).ToArray();
            for (var i = 0; i < raritySortedList.Length; i++)
            {
                raritySortedList[i].transform.SetSiblingIndex(i);
            }
        }
        
        public virtual void SortAscended()
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