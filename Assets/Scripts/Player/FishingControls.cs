using System;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Player
{
    public class FishingControls : MonoBehaviour
    {
        [SerializeField] private int hookChance;
        [SerializeField] private int minimumTimer;
        [SerializeField] private int maximumTimer;

        [SerializeField] private string castRodText = "Tap the screen to cast your rod";
        [SerializeField] private string waitingForBiteText = "Waiting for a bite...";
        [SerializeField] private string nibbleText = "Just a nibble... Waiting  for a bite...";
        [SerializeField] private string biteText = "Got a catchable on the hook! Tap to reel it in!";
        
        private float timeRemaining;
        private bool isRodCast;
        private bool isTimerSet;
        private bool fishBite;

        private IMessageHandler eventsBroker;

        [SerializeField] private Transform floatNoBite;
        [SerializeField] private Transform floatNibbleOrBite;
        [SerializeField] private Text statusText;

        private bool UsesFloat => floatNoBite != null && floatNibbleOrBite != null;
        
        private void Start()
        {
            statusText.text = castRodText;
            
            eventsBroker = FindObjectOfType<EventsBroker>();

            if (UsesFloat)
            {
                floatNoBite.gameObject.SetActive(false);
                floatNibbleOrBite.gameObject.SetActive(false);    
            }

            isRodCast = false;
            isTimerSet = false;
            
            eventsBroker.SubscribeTo<FishAgainEvent>(ReturnFromMinigame);
            eventsBroker.SubscribeTo<EndFishOMeterEvent>(BlankOut);
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isRodCast) CastRod();

            if (isTimerSet)
            {
                if(timeRemaining <= 0)
                {
                    CheckFishBite();    
                }
                else
                {
                    timeRemaining -= Time.deltaTime;
                }
            }
        }
        

        private void CastRod()
        {
            eventsBroker.Publish(new PlaySoundEvent(SoundType.Sfx, "CastingSound"));
            isRodCast = true;
            if (UsesFloat) floatNoBite.gameObject.SetActive(true);

            statusText.text = waitingForBiteText;
            RandomizeTimer();
        }
        
        private void RandomizeTimer()
        {
            timeRemaining = Random.Range(minimumTimer, maximumTimer+1);
            isTimerSet = true;
        }

        private void CheckFishBite()
        {
            var result = Random.Range(0, 100);

            if (result <= hookChance) fishBite = true;
            
            if (!fishBite)
            {
                statusText.text = nibbleText;
                RandomizeTimer();
            }
            else
            {
                statusText.text = biteText;
                ActivateMinigame();
            }
        }

        private void ActivateMinigame()
        {
            ResetValues();

            if (UsesFloat)
            {
                floatNoBite.gameObject.SetActive(false);
                floatNibbleOrBite.gameObject.SetActive(true);    
            }
            
            eventsBroker.Publish(new StartFishOMeterEvent());
        }

        private void ReturnFromMinigame(FishAgainEvent eventRef)
        {
            statusText.text = castRodText;
            isRodCast = false;
        }

        private void BlankOut(EndFishOMeterEvent eventRef)
        {
            if (UsesFloat)
            {
                floatNoBite.gameObject.SetActive(false);
                floatNibbleOrBite.gameObject.SetActive(false);    
            }
            
            statusText.text = "";
        }

        private void ResetValues()
        {
            isTimerSet = false;
            timeRemaining = 0;
        }

        private void OnDestroy()
        {
            eventsBroker?.UnsubscribeFrom<EndFishOMeterEvent>(BlankOut);
            eventsBroker?.UnsubscribeFrom<FishAgainEvent>(ReturnFromMinigame);
        }
    }
}