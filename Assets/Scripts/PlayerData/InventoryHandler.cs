using System;
using EventManagement;
using Events;
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

        public IInventory CurrentInventory => inventory;
        private void Start()
        {
            inventory = new Inventory(new InventorySaver(new PlayerPrefsSaver(), new JsonSerializer()), FindObjectOfType<EventsBroker>());
            LoadInventory();
            PrintInventoryContent();
            var eventBroker = FindObjectOfType<EventsBroker>();
            if (eventBroker != null)
                FindObjectOfType<EventsBroker>().SubscribeTo<EndFishOMeterEvent>(fishCaught => PrintInventoryContent());
            eventBroker.Publish(new EnableInventoryEvent(CurrentInventory));
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