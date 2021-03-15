using EventManagement;
using Events;
using Items.Gear;
using UnityEngine;

namespace Items
{
    public class Hands : MonoBehaviour, IEquipmentSlot
    {
        public IEquippable equippedItem { get; private set; }
        [SerializeField] private EquipmentType myPreferredEquipment;

        public IMessageHandler eventsBroker;
        
        private void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            eventsBroker.SubscribeTo<CheckAndDoEquipEvent>(DoEquip);
        }

        public void DoEquip(CheckAndDoEquipEvent eventRef)
        {
            var itemToEquip = eventRef.Item;
            if (itemToEquip.equipmentType.name.Equals(myPreferredEquipment.name))
            {
                if (equippedItem == null) UnEquipAndEquip(itemToEquip);
                if (itemToEquip.ID == equippedItem.ID) return;
                UnEquipAndEquip(itemToEquip);
            }
        }

        private void UnEquipAndEquip(Equipment itemToEquip)
        {
            equippedItem = null;
            equippedItem = itemToEquip;
            Debug.Log($"Equipped {equippedItem.Name}!");
        }
    }
}