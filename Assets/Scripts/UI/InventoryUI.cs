using System;
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
        public List<Slot> inventorySlots = new List<Slot>();
        private bool decended;
        private int inventorySize;

        public InventorySlot selectedSlot;

        protected virtual void OnEnable()
        { 
            Clear();
            FindObjectOfType<EventsBroker>().SubscribeTo<EnableInventoryEvent>(Setup);
            FindObjectOfType<EventsBroker>().SubscribeTo<UpdateInventoryEvent>(OnUpdateInventoryUI);
            FindObjectOfType<EventsBroker>().Publish(new RequestInventoryData());
        }

        protected virtual void Clear()
        {
            foreach (var child in transform.GetComponentsInChildren<Transform>())
            {
                if (child != transform)
                {
                    Destroy(child.gameObject);
                }
            }

            inventorySlots = new List<Slot>();
        }

        protected virtual void Setup(EnableInventoryEvent inventoryEvent)
        {
            Setup(inventoryEvent.Inventory);
        }

        protected virtual void OnUpdateInventoryUI(UpdateInventoryEvent updateInventoryEvent)
        {
            var item = updateInventoryEvent.Item;
            if (updateInventoryEvent.Added)
            {
                var instance = Instantiate(slotPrefab, gridParent);
                if (AllItems.ItemIndexer.indexer.ContainsKey(item.ID))
                    instance.Setup(AllItems.ItemIndexer.indexer[item.ID] as IItem);
                else
                {
                    var gearItem = updateInventoryEvent.GearInventory.GeneratedGear[item.ID];
                    instance.Setup(gearItem);
                    
                }
                inventorySlots.Add(instance); 
                Destroy(transform.GetChild(inventorySize - 1).gameObject);
            }
            else
            {
                if (item is IOpenable openeable)
                {
                    var removedItems = inventorySlots.Find(slot =>
                    {
                        var inventorySlot = slot as InventorySlot;
                        return inventorySlot.opened;
                    });
                    inventorySlots.Remove(removedItems);
                }
                else
                {
                    var removedItem = inventorySlots.Find(slot => slot.Item == updateInventoryEvent.Item);
                    inventorySlots.Remove(removedItem);
                }
            }
            Resort();
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
            SortDescended();
            for (var i = inventorySlots.Count; i < inventory.MaxSize; i++)
            {
                var instance = Instantiate(slotPrefab, gridParent);
            }

            inventorySize = inventory.MaxSize;
        }

        public virtual void ToggleSort()
        {
            if (decended)
                SortAscended();
            else
                SortDescended();
            decended = !decended;
        }

        public virtual void Resort()
        {
            if (decended)
                SortDescended();
            else
                SortDescended();
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

        private void OnDisable()
        {
            var inspectionArea = FindObjectOfType<ItemInspectionArea>(true);
            inspectionArea.Clear();
            inspectionArea.gameObject.SetActive(false);
            FindObjectOfType<EventsBroker>()?.UnsubscribeFrom<EnableInventoryEvent>(Setup);
            FindObjectOfType<EventsBroker>()?.UnsubscribeFrom<UpdateInventoryEvent>(OnUpdateInventoryUI);
        }

        private void OnDestroy()
        {
            InventorySlot.ResetPanelValues();
        }
    }
}
