using EventManagement;
using UnityEngine;

namespace UI
{
    public class FishPositionUI : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
        [SerializeField] private RectTransform parentRectTransform;
    
        private float fullSize;        
        
        void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            parentRectTransform = GetComponent<RectTransform>();
        
            fullSize = parentRectTransform.rect.width;
            
            eventsBroker.SubscribeTo<UpdateFishUIPositionEvent>(UpdatePosition);
        }

        private void UpdatePosition(UpdateFishUIPositionEvent eventRef)
        {
            var currentPosition = parentRectTransform.anchoredPosition;
            parentRectTransform.anchoredPosition = new Vector3(eventRef.positionX * 100, currentPosition.y);
        }
    }
}
