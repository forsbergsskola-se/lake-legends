using Items;

namespace UI
{
    public class EquippedSlot : Slot
    {
        public override void Setup(IItem item, bool hasCaught = true)
        {
            Item = item;
            ApplyImages();
        }
    }
}