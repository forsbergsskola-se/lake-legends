using System;
using System.Collections;
using EventManagement;
using Events;
using Fish;
using Items;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class FishOMeter : MonoBehaviour
    {
        [SerializeField] private float fishingTime = 10;
        [SerializeField] private float targetBarSpeedMultiplier = 0.2f;
        [SerializeField] private float fishLeftSpeed = 0.3f;
        [SerializeField] private float fishRightSpeed = 1.3f;
        [SerializeField] private Factory factory;
        [SerializeField] private FishOMeterUI fishOMeterUI;
        [SerializeField] private GameEndUI gameEndUI;
        [SerializeField] private GameObject fishOMeterMinigamePanel;

        private float directionMod;
        private float successMeter;
        private float currentCaptureZoneTime;
        private float captureZonePosition;
        private float fishPositionCenterPoint;
        private float minimumZone;
        private float maximumZone;
        private float minimumFishZone;
        private float maximumFishZone;
        
        private bool isMoving;
        private bool gameRunning;
        private bool captureZoneStopped;

        private ICatchable catchable;
        private IMessageHandler eventsBroker;
        private float fishSpeedMagnitudeValue;

        private float captureZoneWidth;
        private float fishPercentMod;

        private bool FishIsInZone => fishPositionCenterPoint <= captureZonePosition + captureZoneWidth / 2 &&
                                     fishPositionCenterPoint >= captureZonePosition - captureZoneWidth / 2;

        
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
                
                if(Input.GetMouseButton(0)) isMoving = true;
                else isMoving = false;
            }
        }

        private void SetupGameplayArea(StartFishOMeterEvent eventTrigger)
        {
            successMeter = 3.0f;
            
            catchable = factory.GenerateFish();
            
            // TODO: Replace this base value with the corresponding RodStat base value
            captureZoneWidth = 1f;
            
            fishPercentMod = Mathf.Abs((catchable.CatchableStrength / 100));
            fishSpeedMagnitudeValue = catchable.CatchableSpeed * targetBarSpeedMultiplier;

            
            // TODO: Include the AccuracyModifier from the equipped Fishing rod
            captureZoneWidth = captureZoneWidth * fishPercentMod;
            
            minimumZone = 0 + captureZoneWidth  / 2;
            maximumZone = 1 - captureZoneWidth  / 2;
            
            
            // TODO: Replace this base value with the FishItem icon width
            var fishWidth = (1f / 30f) * 2f;
            
            minimumFishZone = 0 + ((fishWidth / 2));
            maximumFishZone = 1 - ((fishWidth / 2));

            InitializeCaptureZone();
            InitializeFishSpawnPoint();
            
            fishOMeterMinigamePanel.gameObject.SetActive(true);
            eventsBroker.Publish(new UpdateCaptureZoneUISizeEvent(fishPercentMod, captureZoneWidth));
            eventsBroker.Publish(new UpdateFishZoneUISizeEvent(fishWidth));
            gameRunning = true;
        }
        
        private void InitializeCaptureZone()
        {
            captureZonePosition = Random.Range(0f, 1f);
            StartCoroutine(ChooseRandomDirection(catchable.RandomMoveTimeRange.Randomize()));
        }

        private void InitializeFishSpawnPoint()
        {
            fishPositionCenterPoint = 0.5f;
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
            if (isMoving) directionMod = fishRightSpeed; 
            else directionMod = -fishLeftSpeed;

            fishPositionCenterPoint = Mathf.Clamp(fishPositionCenterPoint + (directionMod * (Time.deltaTime)),
                minimumFishZone,
                maximumFishZone);
            
            eventsBroker.Publish(new UpdateFishUIPositionEvent(fishPositionCenterPoint));
            Debug.Log(Input.touchPressureSupported);
        }
        
        private void UpdateCaptureZonePosition()
        {
            if (captureZoneStopped) 
                return;
            currentCaptureZoneTime += (Time.deltaTime * fishSpeedMagnitudeValue);
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
            PlayerPrefs.SetInt("Debug-CaughtFish", PlayerPrefs.GetInt("Debug-CaughtFish", 0) + 1);
            DebugCaughtLost();
            fishOMeterMinigamePanel.gameObject.SetActive(false);
            gameRunning = false;
            EndGame();
            catchable = null;
        }
        
        private void FishEscape()
        {
            PlayerPrefs.SetInt("Debug-EscapedFish", PlayerPrefs.GetInt("Debug-EscapedFish", 0) + 1);
            DebugCaughtLost();
            fishOMeterMinigamePanel.gameObject.SetActive(false);
            gameRunning = false;
            catchable = null;
            EndGame();
        }

        public void DebugCaughtLost()
        {
            Debug.Log("Caught: " + PlayerPrefs.GetInt("Debug-CaughtFish"));
            Debug.Log("Lost: " + PlayerPrefs.GetInt("Debug-EscapedFish"));
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

            StopAllCoroutines();
            ShowEndGameUI();
        }

        private void ShowEndGameUI()
        {
            gameEndUI.gameObject.SetActive(true);
            gameEndUI.eventsBroker = this.eventsBroker;
            gameEndUI.catchable = this.catchable;
            
            eventsBroker.Publish(new EndFishOMeterEvent(catchable));
            fishOMeterMinigamePanel.SetActive(false);
        }

        private IEnumerator Stop(float time)
        {
            captureZoneStopped = true;
            yield return new WaitForSeconds(time);
            StartCoroutine(ChooseRandomDirection(catchable.RandomMoveTimeRange.Randomize()));
        }

        private IEnumerator ChooseRandomDirection(float time)
        {
            captureZoneStopped = false;
            if (Random.Range(0, 2) == 0)
            {
                var temp = maximumZone;
                maximumZone = minimumZone;
                minimumZone = temp;
                currentCaptureZoneTime = Mathf.Lerp(1, 0, currentCaptureZoneTime);
            }
            yield return new WaitForSeconds(time);
            StartCoroutine(Stop(catchable.RandomStopTimeRange.Randomize()));
        }
    }
}