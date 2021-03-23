using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GoldUI : MonoBehaviour
    {
        private IMessageHandler eventsBroker;
        [SerializeField] private Text goldUIText;
    
        void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<UpdateGoldUIEvent>(UpdateUI);
            eventsBroker.Publish(new RequestGoldData());
        }

        private void UpdateUI(UpdateGoldUIEvent eventRef)
        {
            goldUIText.text = eventRef.Gold.ToString();
        }

        private void OnDestroy()
        {
            eventsBroker.UnsubscribeFrom<UpdateGoldUIEvent>(UpdateUI);
        }
    }
}
