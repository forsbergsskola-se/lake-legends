using Items.Gear;
using PlayerData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
    public class InventorySlot : Slot, IPointerClickHandler
    {
        public override void Setup(IItem item)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
        }
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Item != null)
            {
                var gearInfoPanel = FindObjectOfType<GearInfoPanel>();
                gearInfoPanel.panel.SetActive(true);
                gearInfoPanel.Setup(Item as GearInstance);
            }
        }
    }
}