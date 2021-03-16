using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerData;

namespace Saving
{
    public class GearSaver : IGearSaver
    {
        private readonly ISaver saver;
        private readonly ISerializer serializer;

        public GearSaver(ISaver saver, ISerializer serializer)
        {
            this.saver = saver;
            this.serializer = serializer;
        }
        
        public void SaveGear(string key, GearInventory inventory)
        {
            var serializeObject = serializer.SerializeObject(inventory.GeneratedGear);
            saver.Save(key, serializeObject);
        }

        public async Task<Dictionary<string, GearInstance>> LoadGear(string key)
        {
            var value = await saver.Load(key, serializer.SerializeObject(new Dictionary<string, GearInstance>()));
            return serializer.DeserializeObject<Dictionary<string, GearInstance>>(value);
        }
    }
}