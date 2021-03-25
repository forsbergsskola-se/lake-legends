using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class FisherDexSlot : Slot, IPointerClickHandler
    {
        private bool hasCaught;
        public override void Setup(IItem item, bool hasCaught = true)
        {
            this.hasCaught = hasCaught;
            Item = item;
            var image = GetComponentInChildren<Image>();
            image.sprite = (item as FishItem).type.sprite;
            if (!hasCaught)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (hasCaught)
                FindObjectOfType<BioArea>().Setup(Item as FishItem);
        }
    }
}