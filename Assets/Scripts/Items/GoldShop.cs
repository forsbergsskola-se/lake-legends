using EventManagement;
using Events;
using UnityEngine;

namespace Items
{
    public class GoldShop : MonoBehaviour, IShop
    {
        [SerializeField] private int amount = 10;
    
        private IMessageHandler eventsBroker;

        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
        }

        public void Buy()
        {
            eventsBroker.Publish(new IncreaseGoldEvent(amount));
        }
    }
}
