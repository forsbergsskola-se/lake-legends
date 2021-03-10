using EventManagement;
using Items;
using Newtonsoft.Json;
using Saving;
using UnityEngine;
using JsonSerializer = Saving.JsonSerializer;

namespace PlayerData
{
    public class InventoryHandler : MonoBehaviour
    {
        private IInventory inventory;
        private void Start()
        {
            inventory = new Inventory(new InventorySaver(new PlayerPrefsSaver(), new JsonSerializer()), FindObjectOfType<EventsBroker>());
            LoadInventory();
            PrintInventoryContent();
        }

        private void OnDestroy()
        {
            inventory.Serialize();
        }

        private void LoadInventory()
        {
            inventory.Deserialize();
        }

        public void AddItemToInventory(IItem item)
        {
            inventory.AddItem(item);
            PrintInventoryContent();
        }

        private void PrintInventoryContent()
        {
            var content = JsonConvert.SerializeObject(inventory.GetAllItems(), Formatting.Indented);
            Debug.Log($"List Of Inventory Items {content}");
        }
    }
}