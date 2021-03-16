using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<Dictionary<string, int>> LoadInventory(string key)
        {
            var saveFile = await saver.Load(key, serializer.SerializeObject(new Dictionary<string, int>()));
            return serializer.DeserializeObject<Dictionary<string, int>>(saveFile);
        }
    }
}