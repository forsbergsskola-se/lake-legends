using EventManagement;
using UnityEngine;
using Tutorial.Events;

namespace Tutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        private IMessageHandler messageHandler;
        private void Start()
        {
            messageHandler = FindObjectOfType<EventsBroker>();
        }
    }
}