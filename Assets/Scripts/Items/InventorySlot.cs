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
            Item?.Use();
        }
    }
}