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

        private bool SlotsAreOccupied => upgradeSlot.gearInstance != null && currentNumberOfItems == fuseSlots.Length;
        private bool AllSlotsMatchType => IsTypeMatch();
        private bool AllSlotsMatchRarity => IsRarityMatch();


        void Start()
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

        private void Update()
        {
            fuseButton.interactable = SlotsAreOccupied && AllSlotsMatchType && AllSlotsMatchRarity && affordAble;
        }

        private bool IsTypeMatch()
        {
            foreach (var type in fuseSlots)
            {
                bool result = upgradeSlot.gearInstance.EquipmentType == type.gearInstance.EquipmentType;
                TypeMatchList.Add(result);
            }

            if (TypeMatchList.Contains(false)) return false;
            return true;
        }
        
        private bool IsRarityMatch()
        {
            // TODO: Add Rarity identifiers to match for if is Legendary
            // If upgradeSlot.gearInstance.Rarity == Legendary => return false;
            
            foreach (var type in fuseSlots)
            {
                
                bool result = upgradeSlot.gearInstance.Rarity == type.gearInstance.Rarity;
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
            if (currentNumberOfItems == fuseSlots.Length)
            {
                Debug.Log(currentNumberOfItems);
                return;
            }
            
            foreach (var item in fuseSlots)
            {
                if (item.gearInstance == null)
                {
                    if (currentGearInstanceIDs.Contains(eventRef.gearInstance.ID))
                    {
                        Debug.Log("That item is already selected");
                        break;
                    }
                    currentGearInstanceIDs.Add(eventRef.gearInstance.ID);
                    currentGearInstanceEquipmentTypes.Add(eventRef.gearInstance.EquipmentType);
                    
                    item.gearInstance = eventRef.gearInstance;
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
            upgradeSlot.gearInstance = eventRef.gearInstance;
            costText.text = costText.text.Replace("[Cost]", upgradeSlot.gearInstance.GetFusionCost().ToString());
            FindObjectOfType<ItemInspectionArea>(true).gameObject.SetActive(false);
            eventBroker.SubscribeTo<UpdateSilverUIEvent>(OnSilverUpdate);
            eventBroker.Publish(new RequestSilverData());
        }

        private void OnSilverUpdate(UpdateSilverUIEvent eventRef)
        {
            affordAble = eventRef.Silver >= upgradeSlot.gearInstance.GetFusionCost();
        }

        public void DoSacrifice()
        {
            upgradeSlot.gearInstance.UpgradeRarity();

            foreach (var item in fuseSlots)
            {
                item.gearInstance.Sacrifice();
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
            upgradeSlot.ClearName();
        }
        
        public void ClearSacrificeSlot()
        {
            foreach (var item in fuseSlots)
            {
                if (item.gearInstance == null)
                    continue;
                if (currentGearInstanceIDs.Contains(item.gearInstance.ID))
                    currentGearInstanceIDs.Remove(item.gearInstance.ID);
                item.gearInstance = null;
                item.ClearName();
            }
            
            currentNumberOfItems = 0;
        }
    }
}