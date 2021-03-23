using System;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Ads
{
    public class AdButton : MonoBehaviour
    {
        public int adCooldownInSeconds = 120;
        private EventsBroker eventsBroker;
        void Awake()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            GetComponent<Button>().onClick.AddListener((() => GetComponent<Button>().interactable = false));
        }
        private void OnEnable()
        {
            eventsBroker.SubscribeTo<GetAdWatchTimeEvent>(OnAdWatchTime);
            eventsBroker.Publish(new RequestAdWatchTimeEvent());
        }
        private void OnAdWatchTime(GetAdWatchTimeEvent obj)
        {
            GetComponent<Button>().interactable = (DateTime.UtcNow - obj.latestAdWatchTime).TotalSeconds > adCooldownInSeconds;
        }

        private void OnDisable()
        {
            eventsBroker.UnsubscribeFrom<GetAdWatchTimeEvent>(OnAdWatchTime);
        }
    }
}