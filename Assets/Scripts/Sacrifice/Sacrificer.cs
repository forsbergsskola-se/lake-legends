using System;
using EventManagement;
using Events;
using UI;
using UnityEngine.UI;
using UnityEngine;

namespace Sacrifice
{
    public class Sacrificer : MonoBehaviour
    {
        UpgradeSlot upgradeSlot;
        SacrificeSlot sacrificeSlot;
        IMessageHandler eventBroker;
        public Image bar;
        public Text levelText;

        private Button sacrificeButton;

        private bool SlotsAreOccupied => upgradeSlot.gearInstance != null && sacrificeSlot.gearInstance != null && !upgradeSlot.gearInstance.GearSaveData.GearLevel.IsMaxLevel;

        void Awake()
        {
            upgradeSlot = gameObject.GetComponentInChildren<UpgradeSlot>();
            sacrificeSlot = gameObject.GetComponentInChildren<SacrificeSlot>();
            sacrificeButton = gameObject.GetComponentInChildren<Button>();
            
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker.SubscribeTo<PlaceInUpgradeSlotEvent>(OnPlaceInUpgradeItem);
            eventBroker.SubscribeTo<PlaceInSacrificeSlotEvent>(OnPlaceInSacrificeItem);

            this.gameObject.SetActive(false);
        }

        private void Update()
        {
            sacrificeButton.interactable = SlotsAreOccupied;
        }

        public void Initialize()
        {
            
        }

        private void OnPlaceInSacrificeItem(PlaceInSacrificeSlotEvent eventRef)
        {
            if (eventRef.gearInstance == upgradeSlot.gearInstance)
                return;
            sacrificeSlot.gearInstance = eventRef.gearInstance;
            var currentLevelInfo = upgradeSlot.gearInstance.GetLevelInfoAfterIncrease(0);
            var levelInfo = upgradeSlot.gearInstance.GetLevelInfoAfterIncrease(sacrificeSlot.gearInstance.GetSacrificeValue());
            if (levelInfo.IsMaxLevel)
                bar.fillAmount = 1;
            else
                bar.fillAmount = levelInfo.DifferenceToNextLevel;
            levelText.text = $"Level {currentLevelInfo.Level} > {levelInfo.Level}";
            FindObjectOfType<ItemInspectionArea>().gameObject.SetActive(false);
        }
        
        private void OnPlaceInUpgradeItem(PlaceInUpgradeSlotEvent eventRef)
        {
            this.gameObject.SetActive(true);
            upgradeSlot.gearInstance = eventRef.gearInstance;
            var currentLevelInfo = upgradeSlot.gearInstance.GetLevelInfoAfterIncrease(0);
            if (currentLevelInfo.IsMaxLevel)
                bar.fillAmount = 1;
            else
                bar.fillAmount = currentLevelInfo.DifferenceToNextLevel;
            levelText.text = $"Level: {currentLevelInfo.Level}";
            if (upgradeSlot.gearInstance.GearSaveData.GearLevel.IsMaxLevel)
            {
                levelText.text = "Max Level";
            }
            FindObjectOfType<ItemInspectionArea>().gameObject.SetActive(false);
        }

        public void DoSacrifice()
        {
            var newLevel = upgradeSlot.gearInstance.IncreaseExp(sacrificeSlot.gearInstance.GetSacrificeValue());

            Debug.Log($"{upgradeSlot.gearInstance.Name} is now Level {upgradeSlot.gearInstance.GearSaveData.GearLevel.Level}!");
            sacrificeSlot.gearInstance.Sacrifice();
            levelText.text = $"Level: {newLevel.Level}";
            if (newLevel.IsMaxLevel)
            {
                levelText.text = "Max Level";
            }


            ClearSacrificeSlot();
        }

        public void Close()
        {
            // TODO: Unsubscribe from any events here?

            ClearUpgradeSlot();
            ClearSacrificeSlot();
            FindObjectOfType<ItemInspectionArea>(true).gameObject.SetActive(false);
            eventBroker.Publish(new SacrificeCloseEvent());
            this.gameObject.SetActive(false);
        }

        public void ClearUpgradeSlot()
        {
            upgradeSlot.gearInstance = null;
            upgradeSlot.ClearName();
        }
        
        public void ClearSacrificeSlot()
        {
            sacrificeSlot.gearInstance = null;
            sacrificeSlot.ClearName();
        }

        private void OnDestroy()
        {
            eventBroker.UnsubscribeFrom<PlaceInUpgradeSlotEvent>(OnPlaceInUpgradeItem);
            eventBroker.UnsubscribeFrom<PlaceInSacrificeSlotEvent>(OnPlaceInSacrificeItem);
        }
    }
}