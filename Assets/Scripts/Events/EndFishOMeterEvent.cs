using Items;

namespace Events
{
    public class EndFishOMeterEvent
    {
        public FishItem fishItem;

        public EndFishOMeterEvent(FishItem fishItem) {
            this.fishItem = fishItem;
        }
    }
}