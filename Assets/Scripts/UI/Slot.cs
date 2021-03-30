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
        protected Sprite defaultBackGround;
        protected Sprite defaultFrame;
        protected Sprite defaultMiddleGround;
        public abstract void Setup(IItem item, bool hasCaught = true);

        public virtual void SetDefaultImages()
        {
            defaultBackGround = background.sprite;
            defaultFrame = frame.sprite;
            defaultMiddleGround = middleGround.sprite;
        }

        public virtual void ApplyImages()
        {
            background.sprite = Item.Rarity.background;
            frame.sprite = Item.Rarity.frame;
            middleGround.sprite = Item.Sprite;
        }

        public virtual void ResetImages()
        {
            background.sprite = defaultBackGround;
            frame.sprite = defaultFrame;
            middleGround.sprite = defaultMiddleGround;
        }
        public IItem Item { get; protected set; }
    }
}