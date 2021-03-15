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
        private FisherDexData fisherDexData;

        public IInventory CurrentInventory => inventory;
        public FisherDexData FisherDexData => fisherDexData;
        private void Start()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
            var inventorySaver = new InventorySaver(new PlayerPrefsSaver(), new JsonSerializer());
            inventory = new Inventory(inventorySaver, eventBroker);
            currency = new Currency(new CurrencySaver(new PlayerPrefsSaver(), new JsonSerializer()), eventBroker);
            fisherDexData = new FisherDexData(inventorySaver, eventBroker);
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
            fisherDexData.Serialize();
        }

        private void LoadInventory()
        {
            inventory.Deserialize();
            currency.Deserialize();
            fisherDexData.Deserialize();
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