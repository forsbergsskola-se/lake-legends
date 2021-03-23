using System;
using System.Collections.Generic;
using Events;
using Items;
using Items.Gear;
using PlayerData;
using Sacrifice;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlot : Slot, IPointerClickHandler
    {
        public bool opened;
        Color defaultColor = Color.white;
        [SerializeField] Image slotImage;
        [SerializeField] Image highlightImage;
        Sacrificer sacrificer;
        

        public override void Setup(IItem item, bool hasCaught = true)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
            if (Item is IEquippable equippable)
            {
                equippable.Equipped += OnEquippedItem;
                equippable.UnEquipped += OnUnEquippedItem;
                equippable.PlaceInUpgrade += OnPlaceInUpgradeItem;
                if (equippable.IsEquipped)
                {
                    OnEquippedItem();
                }
                sacrificer = FindObjectOfType<Sacrificer>(true);
            }
            if (Item is ISellable sellable)
            {
                sellable.Sold += OnItemSold;
            }
        }

        private void OnOpened()
        {
            GetComponentInChildren<Text>().text = "{Empty}";
            ClearInspectionArea();
            opened = true;
            Item = null;
        }

        private void OnItemSold()
        {
            if (Item is IEquippable equippable)
            {
                equippable.Equipped -= OnEquippedItem;
                equippable.UnEquipped -= OnUnEquippedItem;
                equippable.PlaceInUpgrade -= OnPlaceInUpgradeItem;
            }
            GetComponentInChildren<Text>().text = "{Empty}";

            var sellable = Item as ISellable;
            sellable.Sold -= OnItemSold;
            ClearInspectionArea();
        }

        private void OnUnEquippedItem()
        {
            slotImage.color = defaultColor;
        }

        private void OnEquippedItem()
        {
            slotImage.color = Color.green;
        }
        
        private void OnPlaceInUpgradeItem()
        {
            /*var image = GetComponent<Image>();
            image.color = Color.green;*/
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            //Item?.Use();

            var inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI.selectedSlot != null)
            {
                inventoryUI.selectedSlot.UnSelect();
            }
            
            inventoryUI.selectedSlot = this;
            highlightImage.gameObject.SetActive(true);

            if (Item == null) return;
            GenerateButtons();
        }

        void UnSelect()
        {
            highlightImage.gameObject.SetActive(false);
        }

        private void ClearInspectionArea()
        {
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>(true);
            itemInspectionArea.Clear();
        }
        
        public void GenerateButtons()
        {
            // Interfaces
            var delegates = new Dictionary<string, Action>();
            var callBacks = new Dictionary<string, Callback>();
            if (Item is IEquippable equippable)
            {
                delegates.Add("Equip", equippable.Equip);
                delegates.Add("Upgrade", sacrificer.TogglePanelOn);
            }
            if (Item is ISellable sellable)
            {
                delegates.Add("Sell", sellable.Sell);
            }
            if (Item is IOpenable openable)
            {
                callBacks.Add("Open", new Callback(openable.Open, OnOpened));
            }
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>(true);
            itemInspectionArea.gameObject.SetActive(true);
            if (Item is GearInstance gear)
            {
                var stats = gear.GetStats();
                itemInspectionArea.CreateButtons(delegates, callBacks, Item.Name, stats);
            }
            else
                itemInspectionArea.CreateButtons(delegates, callBacks, Item.Name);
            
            // Reflection
            /*
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>();
            var type = Item.GetType();
            var interactMethods = type
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Where(info => info.GetCustomAttributes(typeof(InteractAttribute)).Any()).ToList();
            itemInspectionArea.CreateButtons(Item, interactMethods);*/
        }

        private void OnDestroy()
        {
            if (Item is IEquippable equippable)
            {
                equippable.Equipped -= OnEquippedItem;
                equippable.UnEquipped -= OnUnEquippedItem;
            }

            if (Item is ISellable sellable)
            {
                sellable.Sold -= OnItemSold;
            }
        }
    }
}