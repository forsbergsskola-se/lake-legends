using System.Collections.Generic;
using PlayerData;

namespace Saving
{
    public interface IInventorySaver
    {
        void SaveInventory(string key, IInventory inventory);
        Dictionary<string, int> LoadInventory(string key);
    }
}