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
        void Start()
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
            eventsBroker = FindObjectOfType<EventsBroker>();
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

        public void WatchAdLimit()
        {
            var currentTime = DateTime.UtcNow;
            // 1. limit how many times you can watch the ad// 2. limit how much silver user can get from ads// 3. check time enable button on/off, timestamp
            //display time left on button perhaps, countdown.
        }
        
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (showResult == ShowResult.Finished)
            {
                Debug.Log("ad finished");
                if (givesGold)
                    GiveGoldReward();
                else 
                    GiveSilverReward();
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.Log("damn");
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

       
    }

}

