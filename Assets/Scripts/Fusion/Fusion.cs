using EventManagement;
using Events;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion
{
    public class Fusion : MonoBehaviour
    {
        // Copy of Sacrificer.cs
        
        UpgradeSlot upgradeSlot;
        SacrificeSlot sacrificeSlot;
        IMessageHandler eventBroker;

        private Button sacrificeButton;

        private bool SlotsAreOccupied => upgradeSlot.gearInstance != null && sacrificeSlot.gearInstance != null;

        void Start()
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
            sacrificeSlot.gearInstance = eventRef.gearInstance;
        }
        
        private void OnPlaceInUpgradeItem(PlaceInUpgradeSlotEvent eventRef)
        {
            this.gameObject.SetActive(true);
            upgradeSlot.gearInstance = eventRef.gearInstance;
        }

        public void DoSacrifice()
        {
            //TODO: Make math calculation of Amount of XP given to item
            
            upgradeSlot.gearInstance.GearSaveData.level++;
            
            Debug.Log($"{upgradeSlot.gearInstance.Name} is now Level {upgradeSlot.gearInstance.GearSaveData.level}!");
            sacrificeSlot.gearInstance.Sacrifice();
            
            ClearSacrificeSlot();
        }

        public void Close()
        {
            // TODO: Unsubscribe from any events here?

            ClearUpgradeSlot();
            ClearSacrificeSlot();
            
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
    }
}