using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using Items;
using Saving;

namespace PlayerData
{
    public class Inventory : IInventory
    {
        public int MaxSize => 50;
        public int TotalSizeOfInventory => items.Sum(item => item.Value);
        protected Dictionary<string, int> items = new Dictionary<string, int>();
        private readonly IInventorySaver saver;
        protected virtual string InventoryKey => "Inventory";
        
        
        public Inventory(IInventorySaver saver, IMessageHandler messageHandler)
        {
            this.saver = saver;
            messageHandler?.SubscribeTo<EndFishOMeterEvent>(eve =>
            {
                if (eve.catchItem != null && eve.catchItem is IItem item)
                    AddItem(item);
            });
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

        public virtual void Deserialize()
        {
            var savedInventory = saver.LoadInventory(InventoryKey);
            if (savedInventory == null)
                return;
            items = savedInventory;
        }

        public virtual void Serialize()
        {
            saver.SaveInventory(InventoryKey, this);
        }
    }
}