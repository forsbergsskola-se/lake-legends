using EventManagement;
using Events;
using UnityEngine;

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

        private void Start()
        {
            isRodCast = false;
            isTimerSet = false;
            Debug.Log("Ready to play, press Space to cast your rod");
        }
        
        private void Update()
        {
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
            Debug.Log("Casting rod out");
            isRodCast = true;
            RandomizeTimer();
        }
        
        private void RandomizeTimer()
        {
            timeRemaining = Random.Range(minimumTimer, maximumTimer+1);
            isTimerSet = true;
            Debug.Log("Timer is: " + timeRemaining);
        }

        private void CheckFishBite()
        {
            var result = Random.Range(0, 100);

            if (result <= hookChance) fishBite = true;
            
            if (!fishBite)
            {
                Debug.Log("Just a nibble...");
                RandomizeTimer();
            }
            else
            {
                Debug.Log("Got a fish on the hook!");
                ActivateMinigame();
            }
        }

        private void ActivateMinigame()
        {
            ResetValues();
            
            Debug.Log("Starting FishOMeter Minigame");
            eventsBroker.Publish(new StartFishOMeterEvent());
        }

        private void ResetValues()
        {
            isTimerSet = false;
            timeRemaining = 0;
        }
    }
}