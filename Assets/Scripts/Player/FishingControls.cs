using System;
using System.Collections;
using Audio;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Player
{
    public class FishingControls : MonoBehaviour
    {
        [SerializeField] private int baitCost = 1;
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

        private bool affordable;
        
        private IMessageHandler eventsBroker;
        private AudioManager audioManager;

        [SerializeField] private Transform floatNoBite;
        [SerializeField] private Transform floatNibbleOrBite;
        [SerializeField] private Text statusText;
        [SerializeField] private string castingSound = "CastingSound";

        private bool UsesFloat => floatNoBite != null && floatNibbleOrBite != null;
        
        private void Start()
        {
            statusText.text = castRodText;
            
            eventsBroker = FindObjectOfType<EventsBroker>();
            audioManager = FindObjectOfType<AudioManager>();

            if (UsesFloat)
            {
                floatNoBite.gameObject.SetActive(false);
                floatNibbleOrBite.gameObject.SetActive(false);    
            }

            isRodCast = false;
            isTimerSet = false;
            
            eventsBroker.SubscribeTo<UpdateBaitUIEvent>(ComparePriceAndOwnedBait);
            eventsBroker.SubscribeTo<FishAgainEvent>(ReturnFromMinigame);
            eventsBroker.SubscribeTo<EndFishOMeterEvent>(BlankOut);
        }

        private void ComparePriceAndOwnedBait(UpdateBaitUIEvent eventRef)
        {
            affordable = baitCost <= eventRef.Bait;
        }
        
        private void Update()
        {
            if (isTimerSet)
            {
                if (timeRemaining <= 0)
                {
                    CheckFishBite();    
                }
                else
                {
                    timeRemaining -= Time.deltaTime;
                }
            }
        }

        public void TryToPlayTheGame()
        {
            if (!isRodCast)
            { 
                eventsBroker.Publish(new RequestBaitData());
                if (!CheckAffordability())
                    return;
                StartCoroutine(CastRod());
            }
        }

        private bool CheckAffordability()
        {
            if (!affordable)
            {
                //TODO: Show the "You cannot afford it UI"
                return false;
            }
            return true;
        }

        private IEnumerator CastRod()
        {
            eventsBroker.Publish(new PlaySoundEvent(SoundType.Sfx, "CastingSound"));
            eventsBroker.Publish(new DecreaseBaitEvent(baitCost));

            yield return audioManager.PlaySoundEnumerator(new PlaySoundEvent(SoundType.PlayAndWait, castingSound));

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
            eventsBroker?.UnsubscribeFrom<UpdateBaitUIEvent>(ComparePriceAndOwnedBait);
        }
    }
}