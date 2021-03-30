using Items;
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
            GetComponent<Button>().interactable = hasCaught;
            ApplyImages();
        }

        public override void ApplyImages()
        {
            if (hasCaught)
                middleGround.sprite = Item.Sprite;
            frame.sprite = Item.Rarity.frame;
            background.sprite = Item.Rarity.background;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (hasCaught)
                FindObjectOfType<BioArea>().Setup(Item as FishItem);
        }
    }
}