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
        private ICurrency currency;
        private IMessageHandler eventBroker;

        public IInventory CurrentInventory => inventory;
        private void Start()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
            inventory = new Inventory(new InventorySaver(new PlayerPrefsSaver(), new JsonSerializer()), eventBroker);
            currency = new Currency(new CurrencySaver(new PlayerPrefsSaver(), new JsonSerializer()), eventBroker);
            LoadInventory();
            PrintInventoryContent();
            eventBroker?.SubscribeTo<EndFishOMeterEvent>(OnEndFishing);
            eventBroker?.Publish(new EnableInventoryEvent(CurrentInventory));
        }

        private void OnEndFishing(EndFishOMeterEvent obj)
        {
            if (obj.fishItem == null) return;
            PrintInventoryContent();
            eventBroker.Publish(new IncreaseSilverEvent(obj.fishItem.silverValue));
        }

        private void OnDestroy()
        {
            inventory.Serialize();
            currency.Serialize();
        }

        private void LoadInventory()
        {
            inventory.Deserialize();
            currency.Deserialize();
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