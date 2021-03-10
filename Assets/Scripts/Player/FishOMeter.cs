using EventManagement;
using Events;
using Fish;
using Items;
using UI;
using UnityEngine;

namespace Player
{
    public class FishOMeter : MonoBehaviour
    {
        [SerializeField] private float fishingTime = 10;
        [SerializeField] private Factory factory;
        [SerializeField] private FishOMeterUI fishOMeterUI;
        [SerializeField] private GameObject fishOMeterMinigamePanel;
        
        private int directionMod;
        private float successMeter;
        private float captureZoneTime = 3;
        private float currentCaptureZoneTime;
        private float captureZonePosition;
        private float fishPositionCenterPoint;
        private float minimumZone;
        private float maximumZone;
        private float minimumFishZone;
        private float maximumFishZone;
        
        private bool isMoving;
        private bool gameRunning;

        private FishItem fish;
        private IMessageHandler eventsBroker;
        
        private bool FishIsInZone => fishPositionCenterPoint <= captureZonePosition + (fish.fishStrength  / 5) &&
                                     fishPositionCenterPoint >= captureZonePosition - (fish.fishStrength  / 5);
        

        private void Awake()
        {
            gameRunning = false;
        }

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<StartFishOMeterEvent>(SetupGameplayArea);
        }
        
        private void Update()
        {
            if (gameRunning)
            {
                ChangeSuccessMeterValue();
                UpdateFishPosition();
                UpdateCaptureZonePosition();
                if (successMeter <= 0) FishEscape();
                else if (successMeter >= fishingTime) FishCatch();

                if (Input.GetKey(KeyCode.Space)) isMoving = true;
                else isMoving = false;
            }
        }

        private void SetupGameplayArea(StartFishOMeterEvent eventTrigger)
        {
            successMeter = 3.0f;
            
            fish = factory.GenerateFish();
            
            var width = 0.2f;
            var percent = Mathf.Abs((fish.fishStrength / 100));
            
            
            
            minimumZone = 0 + ((width / 2) * (1 + percent));
            maximumZone = 1 - ((width / 2) * (1 + percent));
            
            var fishWidth = 0.10f;
            minimumFishZone = 0 + ((fishWidth / 2));
            maximumFishZone = 1 - ((fishWidth / 2));

            InitializeCaptureZone();
            InitializeFishSpawnPoint();
            
            fishOMeterMinigamePanel.gameObject.SetActive(true);
            eventsBroker.Publish(new UpdateCaptureZoneUISizeEvent(percent, width));
            gameRunning = true;
        }
        
        private void InitializeCaptureZone()
        {
            captureZonePosition = 0;
        }

        private void InitializeFishSpawnPoint()
        {
            fishPositionCenterPoint = 0f;
        }

        private void ChangeSuccessMeterValue()
        {
            if (FishIsInZone)
            {
                successMeter += Time.deltaTime;    
            }
            else
            {
                successMeter -= Time.deltaTime;    
            }
            successMeter = Mathf.Clamp(successMeter, 0 ,fishingTime);
            fishOMeterUI.successBar.fillAmount = successMeter / fishingTime;
        }

        private void UpdateFishPosition()
        {
            if (isMoving) directionMod = 1; 
            else directionMod = -1;
            
            fishPositionCenterPoint = Mathf.Clamp(fishPositionCenterPoint + Time.deltaTime * directionMod, minimumFishZone, maximumFishZone);
            
            eventsBroker.Publish(new UpdateFishUIPositionEvent(fishPositionCenterPoint));
        }
        
        private void UpdateCaptureZonePosition()
        {
            currentCaptureZoneTime += Time.deltaTime / captureZoneTime;
            captureZonePosition = Mathf.Lerp(minimumZone, maximumZone, currentCaptureZoneTime);

            eventsBroker.Publish(new UpdateCaptureZoneUIPositionEvent(captureZonePosition));

            if (currentCaptureZoneTime >= 1)
            {
                var temp = maximumZone;
                maximumZone = minimumZone;
                minimumZone = temp;
                currentCaptureZoneTime = 0;
            }
        }

        private void FishCatch()
        {
            fishOMeterMinigamePanel.gameObject.SetActive(false);
            gameRunning = false;
            EndGame();
            fish = null;
        }
        
        private void FishEscape()
        {
            fishOMeterMinigamePanel.gameObject.SetActive(false);
            gameRunning = false;
            fish = null;
            EndGame();
        }

        private void EndGame()
        {
            fishPositionCenterPoint = 0;
            captureZonePosition = 0;
            minimumZone = 0;
            maximumZone = 0;
            minimumFishZone = 0;
            maximumFishZone = 0;
            successMeter = 3f;

            eventsBroker.Publish(new EndFishOMeterEvent(fish));
        }
    }
}