using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerData;

namespace Saving
{
    public interface IGearSaver
    {
        void SaveGear(string key, GearInventory inventory);
        Task<Dictionary<string, GearInstance>> LoadGear(string key);
    }
}