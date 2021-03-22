using EventManagement;
using Events;
using LootBoxes;
using UnityEngine;


namespace Items
{
    public class ClamShop : MonoBehaviour, IShop
    {
        public LootBox clamToBuy;
        public int price = 10;
        
        private bool affordable;
        
        private IMessageHandler eventsBroker;

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<UpdateSilverUIEvent>(ComparePriceAndOwnedSilver);
        }

        private void ComparePriceAndOwnedSilver(UpdateSilverUIEvent eventRef)
        {
            Debug.Log(eventRef.Silver);
            affordable = price <= eventRef.Silver;
            Debug.Log(affordable);
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