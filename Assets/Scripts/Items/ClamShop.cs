using EventManagement;
using Events;
using LootBoxes;
using UnityEngine;


namespace Items
{
    public class ClamShop : MonoBehaviour, IShop
    {
        [SerializeField] private bool costsGold;
        
        public LootBox clamToBuy;
        public int price = 10;
        
        private bool affordable;
        
        
        private IMessageHandler eventsBroker;

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<UpdateSilverUIEvent>(ComparePriceAndOwnedSilver);
            eventsBroker.SubscribeTo<UpdateGoldUIEvent>(ComparePriceAndOwnedGold);
        }

        private void ComparePriceAndOwnedGold(UpdateGoldUIEvent eventRef)
        {
            affordable = price <= eventRef.Gold;
        }

        private void ComparePriceAndOwnedSilver(UpdateSilverUIEvent eventRef)
        {
            affordable = price <= eventRef.Silver;
        }

        public void Buy()
        {
            eventsBroker.Publish(new RequestSilverData());
            if (!affordable)
            {
                // Show "You cannot afford it, buy some Silver UI"
                Debug.Log("You cannot afford it");
                return;
            }
            eventsBroker.Publish(new DecreaseSilverEvent(price));
            eventsBroker.Publish(new AddItemToInventoryEvent(clamToBuy));
        }
    }
}