﻿using EventManagement;
using Events;
using Items;
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
        private GearInventory gearInventory;

        public IInventory CurrentInventory => inventory;
        public GearInventory GearInventory => gearInventory;
        public FisherDexData FisherDexData => fisherDexData;
        private void Start()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
            var inventorySaver = new InventorySaver(new PlayerPrefsSaver(), new JsonSerializer());
            gearInventory = new GearInventory(new GearSaver(new PlayerPrefsSaver(), new JsonSerializer()));
            inventory = new Inventory(inventorySaver, eventBroker, gearInventory);
            currency = new Currency(new CurrencySaver(new PlayerPrefsSaver(), new JsonSerializer()), eventBroker);
            fisherDexData = new FisherDexData(inventorySaver, eventBroker);
            LoadInventory();
            eventBroker?.SubscribeTo<EndFishOMeterEvent>(OnEndFishing);
            eventBroker?.Publish(new EnableFisherDexEvent(FisherDexData));
            eventBroker?.Publish(new EnableInventoryEvent(inventory));
        }

        private void OnEndFishing(EndFishOMeterEvent obj)
        {
            if (obj.catchItem == null) return;
            if (obj.catchItem is FishItem fishItem)
            {
                eventBroker.Publish(new IncreaseSilverEvent(fishItem.silverValue));
            }
        }

        private void OnDestroy()
        {
            inventory.Serialize();
            currency.Serialize();
            fisherDexData.Serialize();
            gearInventory.PrintInventory();
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
        }
    }
}