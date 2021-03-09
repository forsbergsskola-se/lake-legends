using System.Collections;
using System.Collections.Generic;
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
}