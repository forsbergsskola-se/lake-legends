using System;
using EventManagement;
using Events;
using PlayerData;
using UI;
using UnityEngine;

namespace Sacrifice
{
    public class Sacrificer : MonoBehaviour
    {
        GearInstance gearInstance;
        UpgradeSlot upgradeSlot;
        SacrificeSlot sacrificeSlot;
        IMessageHandler eventBroker;

        void Start()
        {
            upgradeSlot = gameObject.GetComponentInChildren<UpgradeSlot>();
            sacrificeSlot = gameObject.GetComponentInChildren<SacrificeSlot>();
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker.SubscribeTo<PlaceInUpgradeSlotEvent>(OnPlaceInUpgradeItem);
            this.gameObject.SetActive(false);
        }

        public void Initialize()
        {
            
        }

        private void OnPlaceInUpgradeItem(PlaceInUpgradeSlotEvent eventRef)
        {
            this.gameObject.SetActive(true);
            Debug.Log(eventRef.gearInstance);
            this.gearInstance = eventRef.gearInstance;
            upgradeSlot.CurrentGearInstance = gearInstance.Name;
        }

        public void ClearSacrificeSlot()
        {
            
        }
    }
}