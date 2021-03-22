using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class AdsManager : MonoBehaviour, IUnityAdsListener
    {
        private string gameId = "4059145";
        private bool testMode = true;
        
        void Start()
        {
            Advertisement.Initialize(gameId, testMode);
        }

        public void ShowAd()
        {
            Advertisement.Show();
                
        }


        public void OnUnityAdsReady(string placementId)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsDidError(string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            throw new System.NotImplementedException();
        }
    }

}

