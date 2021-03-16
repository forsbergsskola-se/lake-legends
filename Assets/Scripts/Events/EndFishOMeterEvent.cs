using Items;

namespace Events
{
    public class EndFishOMeterEvent
    {
        public readonly ICatchable catchItem;

        public EndFishOMeterEvent(ICatchable catchItem) {
            this.catchItem = catchItem;
        }
    }
}