using System;
using System.Collections;
using EventManagement;
using Events;
using Fish;
using Items;
using LootBoxes;
using PlayerData;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class FishOMeter : MonoBehaviour
    {
        [SerializeField] private float fishingTime = 10;
        [SerializeField] private float startingSuccessMeter = 3.0f;
        [SerializeField] private float targetBarSpeedMultiplier = 0.2f;
        [SerializeField] private float fishLeftSpeed = 0.3f;
        [SerializeField] private float fishRightSpeed = 1.3f;
        [SerializeField] private Factory factory;
        [SerializeField] private FishOMeterUI fishOMeterUI;
        [SerializeField] private GameEndUI gameEndUI;
        [SerializeField] private GameObject fishOMeterMinigamePanel;
        
        [Header("Sound Related")]
        [SerializeField] private string loseFishSound = "LoseFishSound";
        [SerializeField] private string catchFishSound = "CatchFishSound";

        [Header("Animation Related")]
        [SerializeField] private string IdleAnimation = "Idle";
        [SerializeField] private string CatchAnimation = "Catch";
        [SerializeField] private string BendAnimationName = "Bend";
        [SerializeField] private string TiltAnimationName = "Tilt";
        [SerializeField] private string HookedAnimationName = "Hooked";
        
        
        const int treasureChanceMaxValue = 101;
        [SerializeField] [Range(0,100)] private int treasureChance;

        [SerializeField, Tooltip("Maximum area that a Fish+Accuracy should be able to take up")] private float maximumCaptureZoneWidth = 0.5f;
        [SerializeField, Tooltip("Minimum area that a Fish+Accuracy should be able to take up")] private float minimumCaptureZoneWidth = 0.01f;
        
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
        private PlayerBody playerBody;
        private IMessageHandler eventsBroker;
        private float fishSpeedMagnitudeValue;

        private float captureZoneWidth;
        private float fishPercentMod;
        private float accuracyPercentMod;

        private bool FishIsInZone => fishPositionCenterPoint <= captureZonePosition + DivideByTwo(captureZoneWidth) &&
                                     fishPositionCenterPoint >= captureZonePosition - DivideByTwo(captureZoneWidth);

        private float Attraction
        {
            get
            {
                if (playerBody != null)
                    return playerBody.TotalAttraction;
                return 0;
            }
        }
        
        private float Accuracy
        {
            get
            {
                if (playerBody != null)
                    return playerBody.TotalAccuracy;
                return 0;
            }
        }
        
        private float LineStrength
        {
            get
            {
                if (playerBody != null)
                    return playerBody.TotalLineStrength;
                return 0;
            }
        }

        private float DivideByTwo(float value)
        {
            return value / 2; 
        }
        
        private void Awake()
        {
            gameRunning = false;
        }

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            playerBody = FindObjectOfType<PlayerBody>();
            eventsBroker.SubscribeTo<StartFishOMeterEvent>(SetupGameplayArea);
        }
        
        private void Update()
        {
            if (gameRunning)
            {
                if (Input.GetMouseButton(0)) isMoving = true;
                else isMoving = false;
                
                ChangeSuccessMeterValue();
                UpdateFishPosition();
                UpdateCaptureZonePosition();
                if (successMeter <= 0) FishEscape();
                else if (successMeter >= fishingTime) FishCatch();
            }
        }

        private bool IsTreasureCatch()
        {
            return Random.Range(0, treasureChanceMaxValue) <= treasureChance;
        }

        private void SetupGameplayArea(StartFishOMeterEvent eventTrigger)
        {
            eventsBroker.Publish(new AnimationTriggerEvent(HookedAnimationName));
            successMeter = startingSuccessMeter;

            catchable = /*IsTreasureCatch() ?  lootBox :*/ factory.GenerateFish(Attraction);
            
            captureZoneWidth = 1;
            
            fishPercentMod = catchable.CatchableStrength / 100;
            accuracyPercentMod = (Accuracy * 0.001f);
            
            fishSpeedMagnitudeValue = catchable.CatchableSpeed * targetBarSpeedMultiplier;
            
            captureZoneWidth *= Mathf.Clamp(fishPercentMod * (1 + accuracyPercentMod), minimumCaptureZoneWidth, maximumCaptureZoneWidth);

            minimumZone = 0 + captureZoneWidth / 2;
            maximumZone = 1 - captureZoneWidth / 2;

            // TODO: Replace this base value with the FishItem icon width
            var fishWidth = (1f / 30f) * 2f;

            minimumFishZone = 0 + fishWidth / 2;
            maximumFishZone = 1 - fishWidth / 2;

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
                eventsBroker.Publish(new InFishOMeterZone(true));
            }
            else
            {
                var multiplier = Mathf.Lerp(1,0,LineStrength * 0.001f);
                var successMeterProgress = Time.deltaTime * multiplier;
                successMeter -= successMeterProgress;
                eventsBroker.Publish(new InFishOMeterZone(false));
            }
            successMeter = Mathf.Clamp(successMeter, 0 ,fishingTime);
            fishOMeterUI.successBar.fillAmount = successMeter / fishingTime;
            eventsBroker.Publish(new AnimationBlendEvent(BendAnimationName, successMeter / fishingTime));
        }

        private void UpdateFishPosition()
        {
            if (isMoving) directionMod = fishRightSpeed; 
            else directionMod = -fishLeftSpeed;

            fishPositionCenterPoint = Mathf.Clamp(fishPositionCenterPoint + (directionMod * (Time.deltaTime)),
                minimumFishZone,
                maximumFishZone);
            
            eventsBroker.Publish(new AnimationBlendEvent(TiltAnimationName, fishPositionCenterPoint));
            eventsBroker.Publish(new UpdateFishUIPositionEvent(fishPositionCenterPoint));
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
            eventsBroker.Publish(new AnimationTriggerEvent(CatchAnimation));
            eventsBroker.Publish(new PlaySoundEvent(SoundType.Sfx, catchFishSound));
            PlayerPrefs.SetInt("Debug-CaughtFish", PlayerPrefs.GetInt("Debug-CaughtFish", 0) + 1);
            DebugCaughtLost();
            fishOMeterMinigamePanel.gameObject.SetActive(false);
            gameRunning = false;
            EndGame();
            catchable = null;
        }
        
        private void FishEscape()
        {
            eventsBroker.Publish(new AnimationTriggerEvent(IdleAnimation));
            eventsBroker.Publish(new PlaySoundEvent(SoundType.Sfx, loseFishSound));
            PlayerPrefs.SetInt("Debug-EscapedFish", PlayerPrefs.GetInt("Debug-EscapedFish", 0) + 1);
            DebugCaughtLost();
            fishOMeterMinigamePanel.gameObject.SetActive(false);
            gameRunning = false;
            catchable = null;
            EndGame();
        }
        
        public void DebugCaughtLost()
        {
            #if UNITY_EDITOR
            Debug.Log("Caught: " + PlayerPrefs.GetInt("Debug-CaughtFish"));
            Debug.Log("Lost: " + PlayerPrefs.GetInt("Debug-EscapedFish"));
            #endif
        }

        private void EndGame()
        {
            eventsBroker.Publish(new AnimationBlendEvent(BendAnimationName, 0));
            fishPositionCenterPoint = 0;
            captureZonePosition = 0;
            minimumZone = 0;
            maximumZone = 0;
            minimumFishZone = 0;
            maximumFishZone = 0;
            successMeter = startingSuccessMeter;

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

        private void OnDestroy()
        {
            eventsBroker?.UnsubscribeFrom<StartFishOMeterEvent>(SetupGameplayArea);
        }
    }


    public static class FloatExtension
    {
        public static float DivideByTwo(this float value)
        {
            return value / 2;
        }
    }
}