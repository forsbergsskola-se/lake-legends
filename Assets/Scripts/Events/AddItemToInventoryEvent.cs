using Items;

namespace Events
{
    public class AddItemToInventoryEvent
    {
        public IItem Item;

        public AddItemToInventoryEvent(IItem item) => this.Item = item;
    }
}