using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class FishingControls : MonoBehaviour
    {

        [SerializeField] private int hookChance;
        [SerializeField] private int minimumTimer;
        [SerializeField] private int maximumTimer;

        private float timeRemaining;
        private bool isRodCast;
        private bool isTimerSet;
        private bool fishBite;

        private IMessageHandler eventsBroker;

        [SerializeField] private Image floatNoBite;
        [SerializeField] private Image floatNibbleOrBite;
        [SerializeField] private Text statusText;
        
        private void Start()
        {
            statusText.text = "Press SPACE to cast your rod";
            
            eventsBroker = FindObjectOfType<EventsBroker>();
            
            floatNoBite.gameObject.SetActive(false);
            floatNibbleOrBite.gameObject.SetActive(false);
            
            isRodCast = false;
            isTimerSet = false;
            
            eventsBroker.SubscribeTo<FishAgainEvent>(ReturnFromMinigame);
        }
        
        private void Update()
        {
            // TODO: Change this from Space to Touch Input
            if(Input.GetKeyDown(KeyCode.Space) &&!isRodCast) CastRod();
            
            
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
            isRodCast = true;
            floatNoBite.gameObject.SetActive(true);
            
            statusText.text = "Waiting for a bite...";
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
                statusText.text = "Just a nibble... Waiting  for a bite...";
                RandomizeTimer();
            }
            else
            {
                statusText.text = "Got a fish on the hook!";
                ActivateMinigame();
            }
        }

        private void ActivateMinigame()
        {
            ResetValues();
         
            floatNoBite.gameObject.SetActive(false);
            floatNibbleOrBite.gameObject.SetActive(true);
            
            eventsBroker.Publish(new StartFishOMeterEvent());
        }

        private void ReturnFromMinigame(FishAgainEvent eventRef)
        {
            floatNoBite.gameObject.SetActive(true);
            floatNibbleOrBite.gameObject.SetActive(false);
            
            statusText.text = "Press SPACE to cast your rod";
            isRodCast = false;
        }

        private void ResetValues()
        {
            isTimerSet = false;
            timeRemaining = 0;
        }
    }
}