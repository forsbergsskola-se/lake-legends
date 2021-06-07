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
        private void Start()
        {
            messageHandler = FindObjectOfType<EventsBroker>();
            messageHandler.SubscribeTo<CatchEvent>(OnCatchEvent);
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
    }
}