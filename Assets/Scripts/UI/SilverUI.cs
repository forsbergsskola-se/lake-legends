using System;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SilverUI : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
        [SerializeField] private Text silverUIText;
    
        void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<UpdateSilverUIEvent>(UpdateUI);
            eventsBroker.Publish(new RequestSilverData());
        }

        private void UpdateUI(UpdateSilverUIEvent eventRef)
        {
            silverUIText.text = eventRef.Silver.ToString();
        }

        private void OnDestroy()
        {
            eventsBroker.UnsubscribeFrom<UpdateSilverUIEvent>(UpdateUI);
        }
    }
}
