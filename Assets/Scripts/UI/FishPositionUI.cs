using EventManagement;
using Events;
using UnityEngine;

namespace UI
{
    public class FishPositionUI : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
        [SerializeField] private RectTransform parentRectTransform;
        [SerializeField] private RectTransform captureAreaParentPanel;

        private float minimum;
        private float maximum;
        
        void OnEnable()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();

            eventsBroker.SubscribeTo<UpdateFishZoneUISizeEvent>(UpdateSize);
            eventsBroker.SubscribeTo<UpdateFishUIPositionEvent>(UpdatePosition);
        }

        private void UpdateSize(UpdateFishZoneUISizeEvent eventRef)
        {
            var size = captureAreaParentPanel.sizeDelta.x * eventRef.width;
            
            var sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
            sizeDelta.x = size;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            
            minimum = 0;
            maximum = captureAreaParentPanel.sizeDelta.x;
        }

        private void UpdatePosition(UpdateFishUIPositionEvent eventRef)
        {
            var currentPosition = Mathf.Lerp(minimum,maximum, eventRef.positionX);

            var anchoredPosition = parentRectTransform.anchoredPosition;
            anchoredPosition.x = currentPosition;
            parentRectTransform.anchoredPosition = anchoredPosition;
        }

        private void OnDisable()
        {
            eventsBroker.UnsubscribeFrom<UpdateFishZoneUISizeEvent>(UpdateSize);
            eventsBroker.UnsubscribeFrom<UpdateFishUIPositionEvent>(UpdatePosition);
        }
    }
}
