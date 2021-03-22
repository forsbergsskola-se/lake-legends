using EventManagement;
using Events;
using UnityEngine;

namespace Items.Shop
{
    public class BaitShop : MonoBehaviour
    {
        [SerializeField] private bool costsGold;
        
        public int baitToBuy = 1;
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
            if (baitToBuy == 0)
            {
                Debug.Log("Bait is zero");
                return;
            }
            if (!costsGold)
            {
                eventsBroker.Publish(new RequestSilverData());    
                
                if (!affordable)
                {
                    //TODO: Show "You cannot afford it UI"
                    Debug.Log("You cannot afford it");
                    return;
                }
                Debug.Log("You bought the bait");
                eventsBroker.Publish(new DecreaseSilverEvent(price));
                eventsBroker.Publish(new IncreaseBaitEvent(baitToBuy));
            }
            else
            {
                eventsBroker.Publish(new RequestGoldData());    
                
                if (!affordable)
                {
                    //TODO: Show "You cannot afford it UI"
                    Debug.Log("You cannot afford it");
                    return;
                }
                Debug.Log("You bought the bait");
                eventsBroker.Publish(new DecreaseGoldEvent(price));
                eventsBroker.Publish(new IncreaseBaitEvent(baitToBuy));
            }
        }
    }
}
