using EventManagement;
using UnityEngine;
using Tutorial.Events;

namespace Tutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        private IMessageHandler messageHandler;
        [SerializeField] private TutorialPopup tutorialPopup;
        private void Start()
        {
            messageHandler = FindObjectOfType<EventsBroker>();
        }
        
        private void TryCall(Message message)
        {
            if (!message.WasTriggered)
            {
                var instance = Instantiate(tutorialPopup, FindObjectOfType<Canvas>().transform);
                instance.Setup(message);
            }
        }
    }
}