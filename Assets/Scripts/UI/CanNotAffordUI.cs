using System;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CanNotAffordUI : MonoBehaviour
    {
        [SerializeField] private Text uiText;
        
        private IMessageHandler eventsBroker;
        
        private void Awake()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<CanNotAffordUIEvent>(DisplayUI);
            
            this.gameObject.SetActive(false);
            
            uiText.text = string.Empty;
        }

        private void DisplayUI(CanNotAffordUIEvent eventRef)
        {
            this.gameObject.SetActive(true);
            this.uiText.text = eventRef.Text;
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            eventsBroker.UnsubscribeFrom<CanNotAffordUIEvent>(DisplayUI);
        }
    }
}
