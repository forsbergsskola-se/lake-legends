using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEditor;
using UnityEngine;

namespace PlayerData
{
    public class Inventory
    {
        private List<string> items = new List<string>();
        public int MaxStorage = 50;

        public bool AddItem(string iItem)
        {
            if (items.Count + 1 > MaxStorage)
                return false;
            items.Add(iItem);
            return true;
        }

        public bool RemoveItem(string iItem)
        {
            string foundItem = FindItem(iItem);

            if (foundItem == null)
                return false;

            items.Remove(iItem);
            return true;
        }

        public string FindItem(string iItem)
        {
            foreach (var item in items)
            {
                if (item == iItem)
                {
                    return iItem;
                }
            }

            return null;
        }

        public List<string> GetList()
        {
            return items;
        }
    }
    
    public class TestInventory
    {
        private Dictionary<string, int> items = new Dictionary<string, int>();
        public int MaxStorage = 50;

        private int TotalSizeOfInventory => items.Sum(item => item.Value);

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
            if (items.ContainsKey(iItem.ID))
                items.Remove(iItem.ID);
            return true;
        }

        public Dictionary<string, int> GetAllItems()
        {
            return items;
        }
    }
}