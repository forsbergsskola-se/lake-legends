using System.Collections.Generic;
using Items;

namespace PlayerData
{
    public interface IInventory
    {
        int TotalSizeOfInventory { get; }
        bool AddItem(IItem iItem);
        bool RemoveItem(IItem iItem);
        Dictionary<string, int> GetAllItems();
        void Deserialize();
        void Serialize();
    }
}