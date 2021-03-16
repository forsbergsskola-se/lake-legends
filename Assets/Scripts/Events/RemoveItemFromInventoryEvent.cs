using Items;

namespace Events
{
    public class RemoveItemFromInventoryEvent
    {
        public IItem Item;

        public RemoveItemFromInventoryEvent(IItem item) => this.Item = item;
    }
}