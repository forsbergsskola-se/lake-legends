using System.Collections.Generic;
using PlayerData;

namespace Saving
{
    public interface IGearSaver
    {
        void SaveGear(string key, GearInventory inventory);
        Dictionary<string, GearInstance> LoadGear(string key);
    }
}