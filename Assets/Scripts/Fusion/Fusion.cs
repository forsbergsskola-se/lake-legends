using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using Items;
using Items.Gear;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion
{
    public class Fusion : MonoBehaviour
    {
        // Copy of Sacrificer.cs
        
        UpgradeSlot upgradeSlot;
        [SerializeField] FuseSlot[] fuseSlots;
        [SerializeField] private Text costText;
        private List<string> currentGearInstanceIDs = new List<string>();
        private List<bool> TypeMatchList = new List<bool>();
        private List<bool> RarityMatchList = new List<bool>();
        private List<EquipmentType> currentGearInstanceEquipmentTypes = new List<EquipmentType>();
        IMessageHandler eventBroker;


        private int currentNumberOfItems;
        private Button fuseButton;

        private EquipmentType fusionSlotEquipmentType;
        private bool affordAble;

        private bool SlotsAreOccupied => upgradeSlot.gearInstance != null && currentNumberOfItems == CurrentSlotAmount;
        private bool AllSlotsMatchType => IsTypeMatch();
        private bool AllSlotsMatchRarity => IsRarityMatch();


        void Awake()
        {
            upgradeSlot = gameObject.GetComponentInChildren<UpgradeSlot>();
            fuseSlots = gameObject.GetComponentsInChildren<FuseSlot>();
            fuseButton = gameObject.GetComponentInChildren<Button>();
            
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker.SubscribeTo<PlaceInFuseSlotEvent>(OnPlaceInFusionSacrificeItem);
            eventBroker.SubscribeTo<PlaceInFusionUpgradeSlotEvent>(OnPlaceInUpgradeItem);

            currentNumberOfItems = 0;
            
            
            
            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            eventBroker.SubscribeTo<UpdateSilverUIEvent>(OnSilverUpdate);
        }

        private void Update()
        {
            fuseButton.interactable = SlotsAreOccupied && AllSlotsMatchType && AllSlotsMatchRarity && affordAble;
        }

        private void HideAllSlots()
        {
            foreach (var slot in fuseSlots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        private void ShowSlots(int amount)
        {
            HideAllSlots();
            for (int i = 0; i < amount; i++)
            {
                fuseSlots[i].gameObject.SetActive(true);
            }
        }

        private int CurrentSlotAmount
        {
            get
            {
                if(upgradeSlot.gearInstance != null)
                    return upgradeSlot. gearInstance. Rarity. starAmount;
                return 0;
            }
        }

        private bool IsTypeMatch()
        {
            for (int i = 0; i < CurrentSlotAmount; i++)
            {
                bool result = upgradeSlot.gearInstance.EquipmentType == fuseSlots[i].gearInstance.EquipmentType;
                TypeMatchList.Add(result);
            }

            if (TypeMatchList.Contains(false)) return false;
            return true;
        }
        
        private bool IsRarityMatch()
        {
            for (int i = 0; i < CurrentSlotAmount; i++)
            {
                bool result = upgradeSlot.gearInstance.RarityValue == fuseSlots[i].gearInstance.RarityValue;
                RarityMatchList.Add(result);
            }

            if (RarityMatchList.Contains(false)) return false;
            return true;
        }

        public void Initialize()
        {
            
        }

        private void OnPlaceInFusionSacrificeItem(PlaceInFuseSlotEvent eventRef)
        {
            if (currentNumberOfItems == CurrentSlotAmount)
            {
                return;
            }
            
            for (int i = 0; i < CurrentSlotAmount; i++)
            {
                if (fuseSlots[i].gearInstance == null)
                {
                    if (currentGearInstanceIDs.Contains(eventRef.gearInstance.ID))
                    {
                        break;
                    }
                    currentGearInstanceIDs.Add(eventRef.gearInstance.ID);
                    currentGearInstanceEquipmentTypes.Add(eventRef.gearInstance.EquipmentType);
                    
                    fuseSlots[i].Setup(eventRef.gearInstance);
                    currentNumberOfItems++;
                    break;
                }
            }
            FindObjectOfType<ItemInspectionArea>().gameObject.SetActive(false);
        }
        
        private void OnPlaceInUpgradeItem(PlaceInFusionUpgradeSlotEvent eventRef)
        {
            this.gameObject.SetActive(true);
            currentGearInstanceIDs.Add(eventRef.gearInstance.ID);
            upgradeSlot.Setup(eventRef.gearInstance);
            costText.text = costText.text.Replace("[Cost]", upgradeSlot.gearInstance.GetFusionCost().ToString());
            FindObjectOfType<ItemInspectionArea>(true).gameObject.SetActive(false);
            ShowSlots(CurrentSlotAmount);
            eventBroker.Publish(new RequestSilverData());
        }

        private void OnSilverUpdate(UpdateSilverUIEvent eventRef)
        {
            affordAble = eventRef.Silver >= upgradeSlot.gearInstance.GetFusionCost();
        }

        public void DoSacrifice()
        {
            upgradeSlot.gearInstance.UpgradeRarity();

            for (int i = 0; i < CurrentSlotAmount; i++)
            {
                fuseSlots[i].gearInstance.Sacrifice();
            }
            
            eventBroker.Publish(new DecreaseSilverEvent(upgradeSlot.gearInstance.GetFusionCost()));

            Close();
        }

        public void Close()
        {
            // TODO: Unsubscribe from any events here?

            ClearUpgradeSlot();
            ClearSacrificeSlot();
            ClearLists();
            
            eventBroker.Publish(new FusionCloseEvent());
            FindObjectOfType<ItemInspectionArea>(true).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void ClearLists()
        {
            currentGearInstanceIDs.Clear();
            TypeMatchList.Clear();
            RarityMatchList.Clear();
        }

        public void ClearUpgradeSlot()
        {
            upgradeSlot.gearInstance = null;
            upgradeSlot.ResetImages();
        }
        
        public void ClearSacrificeSlot()
        {
            for (int i = 0; i < fuseSlots.Length; i++)
            {
                if (fuseSlots[i].gearInstance == null)
                    continue;
                if (currentGearInstanceIDs.Contains(fuseSlots[i].gearInstance.ID))
                    currentGearInstanceIDs.Remove(fuseSlots[i].gearInstance.ID);
                fuseSlots[i].gearInstance = null;
                fuseSlots[i].ResetImages();
            }
            
            currentNumberOfItems = 0;
        }

        private void OnDestroy()
        {
            eventBroker.UnsubscribeFrom<PlaceInFuseSlotEvent>(OnPlaceInFusionSacrificeItem);
            eventBroker.UnsubscribeFrom<PlaceInFusionUpgradeSlotEvent>(OnPlaceInUpgradeItem);
        }

        private void OnDisable()
        {
            eventBroker.UnsubscribeFrom<UpdateSilverUIEvent>(OnSilverUpdate);
        }
    }
}