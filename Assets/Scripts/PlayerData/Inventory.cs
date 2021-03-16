using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using Items;
using Saving;
using UnityEngine;

namespace PlayerData
{
    public class Inventory : IInventory
    {
        public int MaxSize => 50;
        public int TotalSizeOfInventory => items.Sum(item => item.Value);
        protected Dictionary<string, int> items = new Dictionary<string, int>();
        protected readonly IInventorySaver saver;
        private readonly GearInventory gearInventory;
        protected virtual string InventoryKey => "Inventory";

        protected Inventory(IInventorySaver saver, IMessageHandler messageHandler)
        {
            this.saver = saver;
        }
        
        
        public Inventory(IInventorySaver saver, IMessageHandler messageHandler, GearInventory gearInventory)
        {
            this.saver = saver;
            this.gearInventory = gearInventory;

            messageHandler.SubscribeTo<AddItemToInventoryEvent>(GetItemToAdd);
            messageHandler.SubscribeTo<RemoveItemFromInventoryEvent>(GetItemToRemove);
            messageHandler.SubscribeTo<EndFishOMeterEvent>(eve =>
            {
                if (eve.catchItem != null && eve.catchItem is IItem item)
                    AddItem(item);
            });
        }

        private void GetItemToAdd(AddItemToInventoryEvent obj)
        {
            if (obj.Item is GearInstance item)
                gearInventory.AddItem(item);
            AddItem(obj.Item);
        }
        
        private void GetItemToRemove(RemoveItemFromInventoryEvent obj)
        {
            if (obj.Item is GearInstance item)
                gearInventory.RemoveItem(item);
            RemoveItem(obj.Item);
        }
        
        public virtual bool AddItem(IItem iItem)
        {
            if (TotalSizeOfInventory >= MaxSize)
                return false;
            if (items.ContainsKey(iItem.ID))
                items[iItem.ID]++;
            else
                items.Add(iItem.ID, 1);
            return true;
        }

        public virtual bool RemoveItem(IItem iItem)
        {
            if (!items.ContainsKey(iItem.ID))
                return false;
            if (items.ContainsKey(iItem.ID))
                items[iItem.ID]--;
            if (items[iItem.ID] <= 0)
                items.Remove(iItem.ID);
            return true;
        }

        public virtual Dictionary<string, int> GetAllItems()
        {
            return items;
        }

        public Dictionary<string, GearInstance> GetGear()
        {
            return gearInventory.GeneratedGear;
        }

        public virtual void Deserialize()
        {
            gearInventory.Deserialize();
            gearInventory.PrintInventory();
            var savedInventory = saver.LoadInventory(InventoryKey);
            if (savedInventory == null)
                return;
            items = savedInventory;
        }

        public virtual void Serialize()
        {
            gearInventory.Serialize();
            saver.SaveInventory(InventoryKey, this);
        }
    }
}