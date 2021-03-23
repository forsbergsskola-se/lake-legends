using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BaitUI : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
        [SerializeField] private Text baitUIText;
    
        void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<UpdateBaitUIEvent>(UpdateUI);
            eventsBroker.Publish(new RequestBaitData());
        }

        private void UpdateUI(UpdateBaitUIEvent eventRef)
        {
            baitUIText.text = eventRef.Bait.ToString();
        }

        private void OnDestroy()
        {
            eventsBroker.UnsubscribeFrom<UpdateBaitUIEvent>(UpdateUI);
        }
    }
}
