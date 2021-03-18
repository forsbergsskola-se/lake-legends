using System.Threading.Tasks;

namespace Saving
{
    public class EquipmentSaver
    {
        private ISaver saver;
        private ISerializer serializer;

        public EquipmentSaver(ISaver saver, ISerializer serializer)
        {
            this.saver = saver;
            this.serializer = serializer;
        }
    
        public void SaveEquipment(string key, string[] equippedItems)
        {
            var serializeObject = serializer.SerializeObject(equippedItems);
            saver.Save(key, serializeObject);
        }

        public async Task<string[]> LoadEquipment(string key)
        {
            var value = await saver.Load(key, serializer.SerializeObject(new string[]{}));
            return serializer.DeserializeObject<string[]>(value);
        }
    }
}
