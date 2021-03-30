using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class Slot : MonoBehaviour
    {
        public Image background;
        public Image frame;
        public Image middleGround;
        public abstract void Setup(IItem item, bool hasCaught = true);

        public virtual void ApplyImages()
        {
            background.sprite = Item.Rarity.background;
            frame.sprite = Item.Rarity.frame;
            middleGround.sprite = Item.Sprite;
        }
        public IItem Item { get; protected set; }
    }
}