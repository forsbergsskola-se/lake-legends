using System.Collections.Generic;
using PlayerData;

namespace Saving
{
    public class InventorySaver : IInventorySaver
    {
        private readonly ISaver saver;
        private readonly ISerializer serializer;

        public InventorySaver(ISaver saver, ISerializer serializer)
        {
            this.saver = saver;
            this.serializer = serializer;
        }
        
        public void SaveInventory(string key, IInventory inventory)
        {
            var items = serializer.SerializeObject(inventory.GetAllItems());
            saver.Save(key, items);
        }

        public Dictionary<string, int> LoadInventory(string key)
        {
            var saveFile = saver.Load(key, null);
            return serializer.DeserializeObject<Dictionary<string, int>>(saveFile);
        }
    }
}