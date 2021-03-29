using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class FisherDexSlot : Slot, IPointerClickHandler
    {
        public Image fishImage;
        public Image rarityImage;
        private bool hasCaught;
        public override void Setup(IItem item, bool hasCaught = true)
        {
            this.hasCaught = hasCaught;
            Item = item;
            if (hasCaught)
                fishImage.sprite = (item as FishItem).type.sprite;
            GetComponent<Button>().interactable = hasCaught;
            rarityImage.sprite = item.Rarity.frame;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (hasCaught)
                FindObjectOfType<BioArea>().Setup(Item as FishItem);
        }
    }
}