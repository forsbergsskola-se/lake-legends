using System.Collections.Generic;
using System.Linq;
using Items;
using Saving;

namespace PlayerData
{
    public class Inventory : IInventory
    {
        private Dictionary<string, int> items = new Dictionary<string, int>();
        private int MaxStorage = 50;
        private IInventorySaver saver;
        private const string InventoryKey = "Inventory";
        public int TotalSizeOfInventory => items.Sum(item => item.Value);

        public Inventory(IInventorySaver saver)
        {
            this.saver = saver;
        }

        public bool AddItem(IItem iItem)
        {
            if (TotalSizeOfInventory + 1 > MaxStorage)
                return false;
            if (items.ContainsKey(iItem.ID))
                items[iItem.ID]++;
            else
                items.Add(iItem.ID, 1);
            return true;
        }

        public bool RemoveItem(IItem iItem)
        {
            if (!items.ContainsKey(iItem.ID))
                return false;
            if (items.ContainsKey(iItem.ID))
                items[iItem.ID]--;
            if (items[iItem.ID] <= 0)
                items.Remove(iItem.ID);
            return true;
        }

        public Dictionary<string, int> GetAllItems()
        {
            return items;
        }

        public void Deserialize()
        {
            var savedInventory = saver.LoadInventory(InventoryKey);
            if (savedInventory == null)
                return;
            items = savedInventory;
        }

        public void Serialize()
        {
            saver.SaveInventory(InventoryKey, this);
        }
    }
}