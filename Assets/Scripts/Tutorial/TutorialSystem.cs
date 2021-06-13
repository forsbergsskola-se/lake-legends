using System;
using System.Collections;
using EventManagement;
using UnityEngine;
using Tutorial.Events;
using UI;

namespace Tutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        private IMessageHandler messageHandler;
        [SerializeField] private TutorialPopup tutorialPopup;
        [SerializeField] private Message catchTutorialEvent;
        [SerializeField] private Message chestTutorialEvent;
        [SerializeField] private Message clamTutorialEvent;
        [SerializeField] private Message gearTutorialEvent;
        [SerializeField] private Message upgradeTutorialEvent;
        [SerializeField] private Message fuseTutorialEvent;
        private void Start()
        {
            messageHandler = FindObjectOfType<EventsBroker>();
            messageHandler.SubscribeTo<CatchEvent>(OnCatchEvent);
            messageHandler.SubscribeTo<ChestEvent>(OnChestTutorialEvent);
            messageHandler.SubscribeTo<ClamEvent>(OnClamTutorialEvent);
            messageHandler.SubscribeTo<GearEvent>(OnGearTutorialEvent);
            messageHandler.SubscribeTo<UpgradeEvent>(OnUpgradeTutorialEvent);
            messageHandler.SubscribeTo<FuseEvent>(OnFuseTutorialEvent);
        }
        
        private void TryCall(Message message)
        {
           StartCoroutine(TryCallDelay(message));
        }
        private void OnCatchEvent(CatchEvent eventRef)
        {
           TryCall(catchTutorialEvent);
        }
        private void OnChestTutorialEvent(ChestEvent eventRef)
        {
           TryCall(chestTutorialEvent);
        }
        private void OnClamTutorialEvent(ClamEvent eventRef)
        {
           TryCall(clamTutorialEvent);
        }
        private void OnGearTutorialEvent(GearEvent eventRef)
        {
           TryCall(gearTutorialEvent);
        }
        private void OnUpgradeTutorialEvent(UpgradeEvent eventRef)
        {
           TryCall(upgradeTutorialEvent);
        }
        private void OnFuseTutorialEvent(FuseEvent eventRef)
        {
           TryCall(fuseTutorialEvent);
        }

        private IEnumerator TryCallDelay(Message message)
        {
           yield return new WaitForSeconds(0.2f);
           DoTryCall(message);
        }
        
        private void DoTryCall(Message message)
        {
           if (!message.WasTriggered)
           {
              var instance = Instantiate(tutorialPopup, FindObjectOfType<MainCanvas>().transform);
              instance.Setup(message);
           }
        }
    }
}