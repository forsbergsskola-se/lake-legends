using EventManagement;
using UnityEngine;
using Tutorial.Events;

namespace Tutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        private IMessageHandler messageHandler;
        [SerializeField] private TutorialPopup tutorialPopup;
        [SerializeField] private Message catchEvent;
        [SerializeField] private Message chest TutorialEvent;
        [SerializeField] private Message clam TutorialEvent;
        [SerializeField] private Message gear TutorialEvent;
        [SerializeField] private Message upgrade TutorialEvent;
        [SerializeField] private Message fuse TutorialEvent;
        private void Start()
        {
            messageHandler = FindObjectOfType<EventsBroker>();
            messageHandler.SubscribeTo<CatchEvent>(OnCatchEvent);
            messageHandler.SubscribeTo<Chest TutorialEvent>(OnChest TutorialEvent);
            messageHandler.SubscribeTo<Clam TutorialEvent>(OnClam TutorialEvent);
            messageHandler.SubscribeTo<Gear TutorialEvent>(OnGear TutorialEvent);
            messageHandler.SubscribeTo<Upgrade TutorialEvent>(OnUpgrade TutorialEvent);
            messageHandler.SubscribeTo<Fuse TutorialEvent>(OnFuse TutorialEvent);
        }
        
        private void TryCall(Message message)
        {
            if (!message.WasTriggered)
            {
                var instance = Instantiate(tutorialPopup);
                instance.Setup(message);
            }
        }
        private void OnCatchEvent(CatchEvent eventRef)
        {
           TryCall(catchEvent);
        }
        private void OnChest TutorialEvent(Chest TutorialEvent eventRef)
        {
           TryCall(chest TutorialEvent);
        }
        private void OnClam TutorialEvent(Clam TutorialEvent eventRef)
        {
           TryCall(clam TutorialEvent);
        }
        private void OnGear TutorialEvent(Gear TutorialEvent eventRef)
        {
           TryCall(gear TutorialEvent);
        }
        private void OnUpgrade TutorialEvent(Upgrade TutorialEvent eventRef)
        {
           TryCall(upgrade TutorialEvent);
        }
        private void OnFuse TutorialEvent(Fuse TutorialEvent eventRef)
        {
           TryCall(fuse TutorialEvent);
        }
    }
}