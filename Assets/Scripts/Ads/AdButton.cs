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
        [SerializeField] private Text timeRemainingText; 
        private EventsBroker eventsBroker;
        void Awake()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            GetComponent<Button>().onClick.AddListener(() =>
            {
                GetComponent<Button>().interactable = false;
                UpdateText(1);
                eventsBroker.Publish(new ShowAdEvent());
            });
        }
        private void OnEnable()
        {
            eventsBroker.SubscribeTo<GetAdWatchTimeEvent>(OnAdWatchTime);
            eventsBroker.Publish(new RequestAdWatchTimeEvent());
        }
        private void OnAdWatchTime(GetAdWatchTimeEvent obj)
        {
            var differenceInSec = (DateTime.UtcNow - obj.LatestAdWatchTime).TotalSeconds;
            var canWatchAd = differenceInSec > adCooldownInSeconds;
            GetComponent<Button>().interactable = canWatchAd;
            if (canWatchAd)
                return;
            UpdateText(differenceInSec);
        }

        private void UpdateText(double secondsPast)
        {
            var timeUntilNextWatch = adCooldownInSeconds - secondsPast;
            var hours = timeUntilNextWatch / 3600;
            var mins = timeUntilNextWatch / 60 % 60;
            timeRemainingText.text = hours > 1 ? $"Come Back In {Mathf.Floor((float)hours)} hours and {Mathf.Floor((float)mins)} mins" : $"Come Back In {mins:F0} mins";
        }

        private void OnDisable()
        {
            eventsBroker.UnsubscribeFrom<GetAdWatchTimeEvent>(OnAdWatchTime);
        }
    }
}