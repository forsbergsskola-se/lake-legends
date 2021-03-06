using EventManagement;
using Events;
using Items.Gear;
using PlayerData;
using UnityEngine;

public class DebugAddItemToInventory : MonoBehaviour
{
    [SerializeField] private Equipment itemToAdd;

    private GearInstance debugGearInstance;
    
    
    private IMessageHandler eventsBroker;
    
    private void Start()
    {
        eventsBroker = FindObjectOfType<EventsBroker>();
    }

    public void DebugAddItemButton()
    {
        eventsBroker.Publish(new AddItemToInventoryEvent(new GearInstance(new GearSaveData(itemToAdd))));
    }

    public void EquipItem()
    {
        debugGearInstance = new GearInstance(new GearSaveData(itemToAdd));
        debugGearInstance.Use();
    }
}
