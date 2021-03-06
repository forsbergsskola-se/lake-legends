using System;
using System.Collections.Generic;
using EventManagement;
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
        Color equippedColor = Color.green;
        [SerializeField] Image highlightImage;
        [SerializeField] private GameObject equippedImage;

        private GearInstance gear;
        private static bool _sacrificeIsOpen;
        private static FusionInformation _fusionInformation;
        private static bool _panelIsOpen;

        private IMessageHandler eventBroker;

        public override void Setup(IItem item, bool hasCaught = true)
        {
            Item = item;
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
            SetDefaultImages();
            ApplyImages();
        }
        
        private void OnOpened()
        {
            ResetImages();
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
            }
            ResetImages();

            var sellable = Item as ISellable;
            Item = null;
            sellable.Sold -= OnItemSold;
            ClearInspectionArea();
        }

        private void OnUnEquippedItem()
        {
            ClearInspectionArea();
            equippedImage.SetActive(false);
        }

        private void OnEquippedItem()
        {
            ClearInspectionArea();
            equippedImage.SetActive(true);
        }
        
        private void OnPlaceInUpgradeItem()
        {
            /*var image = GetComponent<Image>();
            image.color = Color.green;*/
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            //Item?.Use();
            if (Item == null) 
                return;
            
            var inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI.selectedSlot != null)
            {
                inventoryUI.selectedSlot.UnSelect();
            }
            
            inventoryUI.selectedSlot = this;
            highlightImage.gameObject.SetActive(true);
            
            GenerateButtons();
        }

        public void UnSelect()
        {
            highlightImage.gameObject.SetActive(false);
        }

        private void ClearInspectionArea()
        {
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>(true);
            itemInspectionArea.gameObject.SetActive(false);
            itemInspectionArea.Clear();
            UnSelect();
        }
        
        public void GenerateButtons()
        {
            // Interfaces
            var delegates = new Dictionary<string, Action>();
            var callBacks = new Dictionary<string, Callback>();
            if (Item is IEquippable equippable)
            {
                if (!(_panelIsOpen))
                {
                    if (equippable.IsEquipped)
                        delegates.Add("Unequip", equippable.Unequip);
                    else
                        delegates.Add("Equip", equippable.Equip);
                }
            }
            if (Item is ISellable sellable)
            {
                var canSell = true;
                var isEquipped = false;
                if (Item is GearInstance gearInstance)
                {
                    canSell = !(_fusionInformation != null && gearInstance == _fusionInformation.GearInstance);
                    isEquipped = gearInstance.IsEquipped;
                }
                if (canSell && !isEquipped)
                    delegates.Add($"Sell ({sellable.Value})", sellable.Sell);
            }
            if (Item is IOpenable openable)
            {
                callBacks.Add("Open", new Callback(openable.Open, OnOpened));
            }
            var itemInspectionArea = FindObjectOfType<ItemInspectionArea>(true);
            itemInspectionArea.gameObject.SetActive(true);
            if (Item is GearInstance gear)
            {
                this.gear = gear;

                if (_panelIsOpen)
                {
                    if (_sacrificeIsOpen && _fusionInformation?.GearInstance != gear && !gear.IsEquipped)
                    {
                        delegates.Add("Sacrifice", gear.AddToSacrificeArea);
                    }
                    else if (_fusionInformation != null && _fusionInformation.FusionIsOpen 
                                                        && gear.EquipmentType == _fusionInformation.EquipmentType
                                                        && gear.RarityValue == _fusionInformation.RarityValue 
                                                        && gear != _fusionInformation.GearInstance
                                                        && !gear.IsEquipped) 
                    {
                        delegates.Add("Add", gear.AddToFuseSlotArea);
                    }

                    if (_sacrificeIsOpen)
                    {
                        delegates.Add("Close Upgrade", () => FindObjectOfType<Sacrificer>().Close());
                    }

                    if (_fusionInformation != null && _fusionInformation.FusionIsOpen)
                    {
                        delegates.Add("Close Fusion", () => FindObjectOfType<Fusion.Fusion>().Close());
                    }
                }
                else
                {
                    if (_fusionInformation?.GearInstance != gear || _fusionInformation == null)
                    {
                        delegates.Add("Upgrade", DoOpenUpgradeArea);
                        if (gear.RarityValue != 3)
                            delegates.Add("Fusion", DoOpenFusionArea);
                    }
                }
               
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

        private void ResetSacrificeIsOpen(SacrificeCloseEvent eventRef)
        {
            _fusionInformation = new FusionInformation(false);
            _sacrificeIsOpen = false;
            _panelIsOpen = false;
            eventBroker.UnsubscribeFrom<SacrificeCloseEvent>(ResetSacrificeIsOpen);
            eventBroker = null;
        }
        
        private void ResetFusionIsOpen(FusionCloseEvent eventRef)
        {
            _fusionInformation = new FusionInformation(false);
            _panelIsOpen = false;
            eventBroker.UnsubscribeFrom<FusionCloseEvent>(ResetFusionIsOpen);
            eventBroker = null;
        }
        
        private void DoOpenFusionArea()
        {
            var fusionWasOpened = gear.OpenFusionArea();
            if (fusionWasOpened)
            {
                _fusionInformation = new FusionInformation(true, gear, gear.RarityValue, gear.EquipmentType);
            }
            _panelIsOpen = fusionWasOpened;
            
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker.SubscribeTo<FusionCloseEvent>(ResetFusionIsOpen);
        }
        
        private void DoOpenUpgradeArea()
        {
            _sacrificeIsOpen = gear.OpenUpgradeArea();
            if (_sacrificeIsOpen)
            {
                _fusionInformation = new FusionInformation(false, gear);
            }
            _panelIsOpen = _sacrificeIsOpen;
            
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker.SubscribeTo<SacrificeCloseEvent>(ResetSacrificeIsOpen);
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

        public static void ResetPanelValues()
        {
            _fusionInformation = null;
            _panelIsOpen = false;
            _sacrificeIsOpen = false;
        }
    }
}