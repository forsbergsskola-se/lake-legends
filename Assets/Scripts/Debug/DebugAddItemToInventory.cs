using EventManagement;
using Events;
using Items.Gear;
using PlayerData;
using UnityEngine;

public class DebugAddItemToInventory : MonoBehaviour
{
    [SerializeField] private Equipment itemToAdd;

    private IMessageHandler eventsBroker;
    
    private void Start()
    {
        eventsBroker = FindObjectOfType<EventsBroker>();
    }

    public void DebugAddItemButton()
    {
        Debug.Log($"Item to add is {itemToAdd}");
        eventsBroker.Publish(new AddItemToInventoryEvent(new GearInstance(new GearSaveData(itemToAdd))));
    }
}
