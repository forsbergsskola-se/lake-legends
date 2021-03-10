using EventManagement;
using UnityEngine;

namespace UI
{
    public class CaptureZoneUI : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
        [SerializeField] private RectTransform parentRectTransform;
        [SerializeField] private RectTransform captureAreaParentPanel;

        private float minimum;
        private float maximum;
        
        void OnEnable()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();

            eventsBroker.SubscribeTo<UpdateCaptureZoneUISizeEvent>(UpdateSize);
            eventsBroker.SubscribeTo<UpdateCaptureZoneUIPositionEvent>(UpdatePosition);
        }

        private void UpdateSize(UpdateCaptureZoneUISizeEvent eventRef)
        {
            var size = captureAreaParentPanel.sizeDelta.x * eventRef.width;
            
            var sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
            sizeDelta.x = size;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            
            minimum = 0;
            maximum = captureAreaParentPanel.sizeDelta.x;
        }

        private void UpdatePosition(UpdateCaptureZoneUIPositionEvent eventRef)
        {
            var currentPosition = Mathf.Lerp(minimum,maximum, eventRef.positionX);

            var anchoredPosition = parentRectTransform.anchoredPosition;
            anchoredPosition.x = currentPosition;
            parentRectTransform.anchoredPosition = anchoredPosition;
        }

        private void OnDisable()
        {
            eventsBroker.UnsubscribeFrom<UpdateCaptureZoneUISizeEvent>(UpdateSize);
            eventsBroker.UnsubscribeFrom<UpdateCaptureZoneUIPositionEvent>(UpdatePosition);
        }
    }
}
