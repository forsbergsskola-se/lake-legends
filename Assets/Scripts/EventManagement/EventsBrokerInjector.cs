using UnityEngine;

namespace EventManagement
{
    public class EventsBrokerInjector : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
    
        // Start is called before the first frame update
        void Start()
        {
            eventsBroker = gameObject.AddComponent<EventsBroker>();
        }
    }
}
