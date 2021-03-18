using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Items;
using Items.Gear;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlot : Slot, IPointerClickHandler
    {
        public GameObject button;
        private Color defaultColor = Color.white;

        public override void Setup(IItem item)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
            if (Item is IEquippable equippable)
            {
                equippable.Equipped += OnEquippedItem;
                equippable.UnEquipped += OnUnEquippedItem;
                if (equippable.IsEquipped)
                {
                    OnEquippedItem();
                }
            }
            if (Item is ISellable sellable)
            {
                sellable.Sold += OnItemSold;
            }
        }

        private void OnItemSold()
        {
            if (Item is IEquippable equippable)
            {
                equippable.Equipped -= OnEquippedItem;
                equippable.UnEquipped -= OnUnEquippedItem;
            }
            GetComponentInChildren<Text>().text = "{Empty}";

            var sellable = Item as ISellable;
            sellable.Sold -= OnItemSold;
            ClearInspectionArea();
        }

        private void OnUnEquippedItem()
        {
            var image = GetComponent<Image>();
            image.color = defaultColor;
        }

        private void OnEquippedItem()
        {
            var image = GetComponent<Image>();
            image.color = Color.green;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            //Item?.Use();
            if (Item != null)
                GenerateButtons();
        }

        private void ClearInspectionArea()
        {
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>();
            itemInspectionArea.Clear();
        }

        public void GenerateButtons()
        {
            // Interfaces
            var delegates = new Dictionary<string, Action>();
            if (Item is IEquippable equippable)
            {
                delegates.Add("Equip", equippable.Equip);
            }
            if (Item is ISellable sellable)
            {
                delegates.Add("Sell", sellable.Sell);
            }
            if (Item is IOpenable openable)
            {
                delegates.Add("Open", openable.Open);
            }
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>();
            itemInspectionArea.CreateButtons(delegates, Item.Name, Item.ToString());
            
            // Reflection
            /*
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>();
            var type = Item.GetType();
            var interactMethods = type
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Where(info => info.GetCustomAttributes(typeof(InteractAttribute)).Any()).ToList();
            itemInspectionArea.CreateButtons(Item, interactMethods);*/
        }
    }
}