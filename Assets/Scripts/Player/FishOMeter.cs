using EventManagement;
using Fish;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class FishOMeter : MonoBehaviour
    {
        [SerializeField] private float fishingTime = 10;
        [SerializeField] private Factory factory;
        
        [SerializeField] private Text successBarProgressText;
        [SerializeField] private Text fishPositionText;
        [SerializeField] private Text captureZonePositionText;
        
        private int directionMod;
        private float successMeter;
        private float captureZoneTime = 3;
        private float currentCaptureZoneTime;
        private float captureZonePosition;
        private float fishPositionCenterPoint;
        private float minimum;
        private float maximum;
        
        private bool isMoving;

        private FishItem fish;
        private bool FishIsInZone => fishPositionCenterPoint <= captureZonePosition + fish.fishStrength &&
                                     fishPositionCenterPoint >= captureZonePosition - fish.fishStrength;

        private IMessageHandler eventsBroker;
        
        private void Start()
        {
            eventsBroker = gameObject.AddComponent<EventsBroker>();
            successMeter = 3.0f;
            
            fish = factory.GenerateFish();

            minimum = 0 + fish.fishStrength;
            maximum = 1 - fish.fishStrength;
            
            SetupGameplayArea();
        }
        
        private void Update()
        {
            ChangeSuccessMeterValue();
            UpdateFishPosition();
            UpdateCaptureZonePosition();
            captureZonePositionText.text = $"Capture Zone: {captureZonePosition}";
            if (successMeter <= 0) FishEscape();
            else if (successMeter >= fishingTime) FishCatch();

            if (Input.GetKey(KeyCode.Space)) isMoving = true;
            else isMoving = false;
        }

        private void SetupGameplayArea()
        {
            InitializeCaptureZone();
            InitializeFishSpawnPoint();
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
            
            successBarProgressText.text = $"Success Bar: {successMeter}";
        }

        private void UpdateFishPosition()
        {
            if (isMoving) directionMod = 1; 
            else directionMod = -1;
            
            fishPositionCenterPoint = Mathf.Clamp(fishPositionCenterPoint + Time.deltaTime * directionMod, 0f, 1f);
            fishPositionText.text = $"Fish pos: {fishPositionCenterPoint}";
        }
        
        private void UpdateCaptureZonePosition()
        {
            currentCaptureZoneTime += Time.deltaTime / captureZoneTime;
            captureZonePosition = Mathf.Lerp(minimum, maximum, currentCaptureZoneTime);

            if (currentCaptureZoneTime >= 1)
            {
                var temp = maximum;
                maximum = minimum;
                minimum = temp;
                currentCaptureZoneTime = 0;
            }
        }

        private void FishCatch()
        {
            // TODO: Pass on fish to Inventory
            
            Debug.Log($"Caught a {fish.name}");
        }
        
        private void FishEscape()
        {
            fish = null;
            Debug.Log("It got away");
        }
    }
}