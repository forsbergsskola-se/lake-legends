using System;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Ads
{
    public class AdsManager : MonoBehaviour, IUnityAdsListener
    {
        public string gameId = "4059145";
        public bool testMode = true;
        public string placementId = "marketAd";
        public bool givesGold;
        public int rewardAmount = 100;
        
        private IMessageHandler eventsBroker;
        void Awake()
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<ShowAdEvent>(adEvent => ShowAd());
        }
        public void ShowAd()
        {
            Advertisement.Show(placementId);
        }

        private void GiveSilverReward()
        {
            eventsBroker.Publish(new IncreaseSilverEvent(rewardAmount)); 
            eventsBroker.Publish(new SaveAdWatchTimeEvent());
        }

        private void GiveGoldReward()
        {
            eventsBroker.Publish(new IncreaseGoldEvent(rewardAmount)); 
            eventsBroker.Publish(new SaveAdWatchTimeEvent());
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (showResult == ShowResult.Finished)
            {
                if (givesGold)
                    GiveGoldReward();
                else 
                    GiveSilverReward();
            }
            else if (showResult == ShowResult.Failed)
            {
                
            }
        }

        public void OnUnityAdsReady(string placementId)
        {
            
        }

        public void OnUnityAdsDidError(string message)
        {
            
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            
        }

        private void OnDestroy()
        {
            Advertisement.RemoveListener(this);
        }
    }

}

