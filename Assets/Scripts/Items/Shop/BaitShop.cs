using System;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Shop
{
    public class BaitShop : MonoBehaviour
    {
        [SerializeField] private bool costsGold;
        
        public int baitToBuy = 1;
        public int price = 10;
        
        private bool affordable;
        
        private IMessageHandler eventsBroker;
        private Button button;

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            if (costsGold)
            {
                eventsBroker.SubscribeTo<UpdateGoldUIEvent>(ComparePriceAndOwnedGold); 
                eventsBroker.Publish(new RequestGoldData());    
            }
            else
            {
                eventsBroker.SubscribeTo<UpdateSilverUIEvent>(ComparePriceAndOwnedSilver);
                eventsBroker.Publish(new RequestSilverData()); 
            }
        }
        
        private void SetInteractableState()
        {
            button ??= GetComponent<Button>();
            button.interactable = affordable;
        }

        private void ComparePriceAndOwnedGold(UpdateGoldUIEvent eventRef)
        {
            affordable = price <= eventRef.Gold;
            SetInteractableState();
            eventsBroker.Publish(new RequestBaitData());
        }

        private void ComparePriceAndOwnedSilver(UpdateSilverUIEvent eventRef)
        {
            affordable = price <= eventRef.Silver;
            SetInteractableState();
            eventsBroker.Publish(new RequestBaitData());
        }

        public void Buy()
        {
            if (baitToBuy == 0)
            {
                return;
            }
            if (!costsGold)
            {
                eventsBroker.Publish(new RequestSilverData());    
                
                if (CheckAffordability()) return;
                
                eventsBroker.Publish(new DecreaseSilverEvent(price));
                eventsBroker.Publish(new IncreaseBaitEvent(baitToBuy, true));
            }
            else
            {
                eventsBroker.Publish(new RequestGoldData());    
                
                if (CheckAffordability()) return;
                
                eventsBroker.Publish(new DecreaseGoldEvent(price));
                eventsBroker.Publish(new IncreaseBaitEvent(baitToBuy,true));
            }
        }
        
        private bool CheckAffordability()
        {
            if (!affordable)
            {
                //TODO: Show "You cannot afford it UI"
                return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            eventsBroker?.UnsubscribeFrom<UpdateGoldUIEvent>(ComparePriceAndOwnedGold);
            eventsBroker?.UnsubscribeFrom<UpdateSilverUIEvent>(ComparePriceAndOwnedSilver);
        }
    }
}
