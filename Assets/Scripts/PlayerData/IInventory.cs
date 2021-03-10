﻿using System.Collections.Generic;
using Items;

namespace PlayerData
{
    public interface IInventory
    {
        int MaxSize { get; }
        int TotalSizeOfInventory { get; }
        bool AddItem(IItem iItem);
        bool RemoveItem(IItem iItem);
        Dictionary<string, int> GetAllItems();
        void Deserialize();
        void Serialize();
    }
}