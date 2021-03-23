using EventManagement;
using Events;
using UnityEngine;

namespace Items.Shop
{
    public class CurrencyShop : MonoBehaviour, IShop
    {
        [SerializeField] private int amount = 10;

        [SerializeField] private bool givesSilver;
        [SerializeField] private bool costsGold;
        //TODO: IAP: Introduce bool "requiresApprovedPurchase" for when IAP is implemented 
        
        [SerializeField] int price = 10;
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
            if (!costsGold)
            {
                eventsBroker.Publish(new RequestSilverData());    
                
                if (CheckAffordability()) return;
                eventsBroker.Publish(new DecreaseSilverEvent(price));
                GiveCurrency();
            }
            else
            {
                eventsBroker.Publish(new RequestGoldData());    
                
                if (CheckAffordability()) return;
                eventsBroker.Publish(new DecreaseGoldEvent(price));
                GiveCurrency();
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

        private void GiveCurrency()
        {
            if (givesSilver)
            {
                eventsBroker.Publish(new IncreaseSilverEvent(amount));
            }
            else
            {
                eventsBroker.Publish(new IncreaseGoldEvent(amount));
            }
        }
    }
}