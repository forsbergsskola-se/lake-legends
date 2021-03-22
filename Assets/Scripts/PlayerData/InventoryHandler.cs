using System.Collections;
using System.Threading.Tasks;
using EventManagement;
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
        private ISaver saver;

        public IInventory CurrentInventory => inventory;
        public GearInventory GearInventory => gearInventory;
        public FisherDexData FisherDexData => fisherDexData;
        private void Start()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker?.SubscribeTo<LoginEvent>(LoginListener);
        }

        private void LoginListener(LoginEvent obj)
        {
            StartCoroutine(OnLogin(obj));
        }

        private IEnumerator OnLogin(LoginEvent obj)
        {
            if (obj.Debug)
            {
                var inventorySaver = new InventorySaver(new PlayerPrefsSaver(), new JsonSerializer());
                gearInventory = new GearInventory(new GearSaver(new PlayerPrefsSaver(), new JsonSerializer()));
                inventory = new Inventory(inventorySaver, eventBroker, gearInventory);
                currency = new Currency(new CurrencySaver(new PlayerPrefsSaver(), new JsonSerializer()), eventBroker);
                fisherDexData = new FisherDexData(inventorySaver, eventBroker);
            }
            else
            {
                var inventorySaver = new InventorySaver(new DataBaseSaver(obj.User), new JsonSerializer());
                gearInventory = new GearInventory(new GearSaver(new DataBaseSaver(obj.User), new JsonSerializer()));
                inventory = new Inventory(inventorySaver, eventBroker, gearInventory);
                currency = new Currency(new CurrencySaver(new DataBaseSaver(obj.User), new JsonSerializer()), eventBroker);
                fisherDexData = new FisherDexData(inventorySaver, eventBroker);
            }

            var task = LoadInventory();
            while (!task.IsCompleted)
            {
                yield return null;
            }

            saver = obj.Debug ? (ISaver) new PlayerPrefsSaver() : new DataBaseSaver(obj.User);
            eventBroker?.SubscribeTo<EndFishOMeterEvent>(OnEndFishing);
            eventBroker?.SubscribeTo<RequestFisherDexData>(OnFisherDexDataRequest);
            eventBroker?.SubscribeTo<RequestInventoryData>(OnInventoryDataRequest);
            eventBroker?.SubscribeTo<RequestSilverData>(OnSilverDataRequest);
            eventBroker?.Publish(new EnableFisherDexEvent(FisherDexData));
            eventBroker?.Publish(new EnableInventoryEvent(inventory));
            eventBroker?.Publish(new LoadedInventoryEvent(saver, inventory));
        }

        private void OnInventoryDataRequest(RequestInventoryData obj)
        {
            eventBroker?.Publish(new EnableInventoryEvent(inventory));
        }

        private void OnFisherDexDataRequest(RequestFisherDexData obj)
        {
            eventBroker?.Publish(new EnableFisherDexEvent(FisherDexData));
        }

        /*
         *    private int OnCurrencyAmountRequest(RequestCurrencyAmountEvent request)
         *    int currentAmount = currency.Silver/currency.Gold
         *    return currentAmount
         */
        
        private void OnSilverDataRequest(RequestSilverData request)
        {
            eventBroker?.Publish(new UpdateSilverUIEvent(currency.Silver));
        }

        private void OnEndFishing(EndFishOMeterEvent obj)
        {
            if (obj.catchItem == null) return;
            if (obj.catchItem is FishItem fishItem)
            {
                eventBroker.Publish(new IncreaseSilverEvent(fishItem.silverValue));
            }
        }
        
        private async Task LoadInventory()
        {
            await inventory.Deserialize();
            await currency.Deserialize();
            await fisherDexData.Deserialize();
        }

        public void AddItemToInventory(IItem item)
        {
            inventory.AddItem(item);
        }
    }
}