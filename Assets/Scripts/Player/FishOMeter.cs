using EventManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class FishOMeter : MonoBehaviour
    {
        [SerializeField] private float fishingTime = 10;
        
        private float successMeter;

        private float captureZonePosition;
        private float fishPositionCenterPoint;
        
        private int directionMod;
        private bool isMoving;
        private bool FishIsInZone => fishPositionCenterPoint <= captureZonePosition + fishStrength &&
                                     fishPositionCenterPoint >= captureZonePosition - fishStrength;

        private float fishStrength = 0.1f;
        

        [SerializeField] private Text successBarProgressText;
        [SerializeField] private Text fishPositionText;
        [SerializeField] private Text captureZonePositionText;

        //[SerializeField] private Image successBar;
        //[SerializeField] private RectTransform captureZone;
        //[SerializeField] private RectTransform fishPosition;

        private IMessageHandler eventsBroker;
        
        private void Start()
        {
            eventsBroker = gameObject.AddComponent<EventsBroker>();
            successMeter = 3.0f;
            
            SetupGameplayArea();
        }
        
        private void Update()
        {
            ChangeSuccessMeterValue();
            UpdateFishPosition();
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
            captureZonePosition = Random.Range(0f, 1f);
            captureZonePositionText.text = $"Capture Zone: {captureZonePosition}";
            
            // captureZonePosition = Mathf.Clamp(captureZone.anchoredPosition.x,
            //     0 + (captureZone.rect.width / 2),
            //     GetComponentInParent<RectTransform>().rect.width - (captureZone.rect.width / 2));
        }

        private void InitializeFishSpawnPoint()
        {
            // var fishPositionAnchoredPosition = fishPosition.anchoredPosition;
            // fishPositionAnchoredPosition.x = captureZone.anchoredPosition.x;
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

            // var fishPositionAnchoredPosition = fishPosition.anchoredPosition;
            // fishPositionAnchoredPosition.x += (Time.deltaTime * fishPositionSpeedMultiplier) * directionMod;

            // Constantly move fishPositionCenterPoint towards CaptureAreaPanel's XMin-point

            // if isMoving = true, move fish 2x towards CaptureAreaPanel XMax-point
        }

        private void FishCatch()
        {
            Debug.Log("Caught a fish");
        }
        
        private void FishEscape()
        {
            Debug.Log("It got away");
        }
    }
}