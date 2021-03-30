using System;
using EventManagement;
using Events;
using LootBoxes;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Shop
{
    public class ClamShop : MonoBehaviour, IShop
    {
        [SerializeField] private bool costsGold;
        [SerializeField] private int amountToGive = 1;
        
        public LootBox clamToBuy;
        public int price = 10;
        
        private bool affordable;
        private bool inventoryHasSpace;
        private Button button;
        
        private IMessageHandler eventsBroker;

        private void OnEnable()
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
            
            
            eventsBroker.SubscribeTo<InventorySizeEvent>(CheckInventoryFull);
            eventsBroker.Publish(new RequestInventorySizeEvent());
        }

        private void SetInteractableState()
        {
            button ??= GetComponent<Button>();
            button.interactable = inventoryHasSpace && affordable;
        }

        private void CheckInventoryFull(InventorySizeEvent obj)
        {
            inventoryHasSpace = obj.CanFit(amountToGive);
            SetInteractableState();
        }

        private void ComparePriceAndOwnedGold(UpdateGoldUIEvent eventRef)
        {
            affordable = price <= eventRef.Gold;
            SetInteractableState();
        }

        private void ComparePriceAndOwnedSilver(UpdateSilverUIEvent eventRef)
        {
            affordable = price <= eventRef.Silver;
            SetInteractableState();
        }

        public void Buy()
        {
            if (clamToBuy == null)
            {
                return;
            }
            if (!costsGold)
            {
                eventsBroker.Publish(new RequestSilverData());    
                
                if (CheckAffordability()) return;
                
                eventsBroker.Publish(new DecreaseSilverEvent(price));
                for (var i = 0; i < amountToGive; i++)
                {
                    eventsBroker.Publish(new AddItemToInventoryEvent(clamToBuy));
                }
            }
            else
            {
                eventsBroker.Publish(new RequestGoldData());    
                
                if (CheckAffordability()) return;
                
                eventsBroker.Publish(new DecreaseGoldEvent(price));
                for (var i = 0; i < amountToGive; i++)
                {
                    eventsBroker.Publish(new AddItemToInventoryEvent(clamToBuy));
                }
            }
        }

        private void OnDisable()
        {
            eventsBroker?.UnsubscribeFrom<UpdateSilverUIEvent>(ComparePriceAndOwnedSilver);
            eventsBroker?.UnsubscribeFrom<UpdateGoldUIEvent>(ComparePriceAndOwnedGold);
            eventsBroker?.UnsubscribeFrom<InventorySizeEvent>(CheckInventoryFull);
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
    }

    public class RequestInventorySizeEvent
    {
    }

    public class InventorySizeEvent
    {
        private readonly int maxSize;
        private readonly int currentInventorySize;

        public InventorySizeEvent(int maxSize, int currentInventorySize)
        {
            this.maxSize = maxSize;
            this.currentInventorySize = currentInventorySize;
        }

        public bool CanFit(int amount)
        {
            return maxSize - currentInventorySize >= amount;
        }
    }
}