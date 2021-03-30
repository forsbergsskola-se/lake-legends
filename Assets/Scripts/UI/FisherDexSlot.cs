using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FisherDexSlot : Slot
    {
        private bool hasCaught;
        public override void Setup(IItem item, bool hasCaught = true)
        {
            this.hasCaught = hasCaught;
            Item = item;
            background.GetComponent<Button>().interactable = hasCaught;
            ApplyImages();
        }

        public override void ApplyImages()
        {
            if (hasCaught)
            {
                middleGround.sprite = Item.Sprite;
                background.sprite = Item.Rarity.background;
                middleGround.color = Color.white;
            }
            else
            {
                background.rectTransform.anchorMax = Vector2.one;
                background.rectTransform.anchorMin = Vector2.zero;
            }
            frame.sprite = Item.Rarity.frame;
        }

        public void OnClick()
        {
            if (hasCaught)
                FindObjectOfType<BioArea>().Setup(Item as FishItem);
        }
    }
}