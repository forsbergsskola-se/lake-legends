using EventManagement;
using Events;
using LootBoxes;
using UnityEngine;

namespace Items.Shop
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
            if (clamToBuy == null)
            {
                Debug.Log("Clam is null");
                return;
            }
            if (!costsGold)
            {
                eventsBroker.Publish(new RequestSilverData());    
                
                if (CheckAffordability()) return;
                
                eventsBroker.Publish(new DecreaseSilverEvent(price));
                eventsBroker.Publish(new AddItemToInventoryEvent(clamToBuy));
            }
            else
            {
                eventsBroker.Publish(new RequestGoldData());    
                
                if (CheckAffordability()) return;
                
                eventsBroker.Publish(new DecreaseGoldEvent(price));
                eventsBroker.Publish(new AddItemToInventoryEvent(clamToBuy));
            }
        }
        
        private bool CheckAffordability()
        {
            if (!affordable)
            {
                //TODO: Show "You cannot afford it UI"
                Debug.Log("You cannot afford it");
                return true;
            }

            return false;
        }
    }
}