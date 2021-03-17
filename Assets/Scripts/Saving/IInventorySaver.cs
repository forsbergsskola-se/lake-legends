using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerData;

namespace Saving
{
    public interface IInventorySaver
    {
        void SaveInventory(string key, IInventory inventory);
        Task<Dictionary<string, int>> LoadInventory(string key);
    }
}