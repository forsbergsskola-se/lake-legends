using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
    public class FisherDexSlot : Slot, IPointerClickHandler
    {
        public override void Setup(IItem item)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Item != null)
                FindObjectOfType<BioArea>().Setup(Item as FishItem);
        }
    }
}