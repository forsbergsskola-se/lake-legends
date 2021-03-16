using System.Collections.Generic;
using System.Threading.Tasks;
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
        Dictionary<string, GearInstance> GetGear();
        Task Deserialize();
        void Serialize();
    }
}