using Auth;
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
            eventBroker?.SubscribeTo<LoginEvent>(OnLogin);
            eventBroker?.SubscribeTo<EndFishOMeterEvent>(OnEndFishing);
        }

        private void OnLogin(LoginEvent obj)
        {
            if (obj.Debug)
            {
                var inventorySaver = new InventorySaver(new PlayerPrefsSaver(), new JsonSerializer());
                inventory = new Inventory(inventorySaver, eventBroker);
                currency = new Currency(new CurrencySaver(new PlayerPrefsSaver(), new JsonSerializer()), eventBroker);
                fisherDexData = new FisherDexData(inventorySaver, eventBroker);
            }
            else
            {
                var inventorySaver = new InventorySaver(new DataBaseSaver(obj.User), new JsonSerializer());
                inventory = new Inventory(inventorySaver, eventBroker);
                currency = new Currency(new CurrencySaver(new DataBaseSaver(obj.User), new JsonSerializer()), eventBroker);
                fisherDexData = new FisherDexData(inventorySaver, eventBroker);
            }
            LoadInventory();
            eventBroker?.Publish(new EnableInventoryEvent(FisherDexData));
        }

        private void OnEndFishing(EndFishOMeterEvent obj)
        {
            if (obj.catchItem == null) return;
            PrintInventoryContent();
            if (obj.catchItem is FishItem fishItem)
            {
                eventBroker.Publish(new IncreaseSilverEvent(fishItem.silverValue));
            }
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