using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CaptureZoneUI : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
        [SerializeField] private RectTransform parentRectTransform;
        [SerializeField] private RectTransform captureAreaParentPanel;
        [SerializeField] private Image highlightImage;

        private float minimum;
        private float maximum;
        
        void OnEnable()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();

            eventsBroker.SubscribeTo<UpdateCaptureZoneUISizeEvent>(UpdateSize);
            eventsBroker.SubscribeTo<UpdateCaptureZoneUIPositionEvent>(UpdatePosition);
            eventsBroker.SubscribeTo<InFishOMeterZone>(Callback);
        }

        private void Callback(InFishOMeterZone obj)
        {
            highlightImage.enabled = obj.State;
        }

        private void UpdateSize(UpdateCaptureZoneUISizeEvent eventRef)
        {
            var size = captureAreaParentPanel.sizeDelta.x * eventRef.width;
            
            var sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
            sizeDelta.x = size;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            parentRectTransform.sizeDelta = sizeDelta;
            highlightImage.rectTransform.sizeDelta = sizeDelta;
            
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
