namespace Events
{
    public class UpdateBaitUIEvent
    {
        public readonly int Bait;
        
        public UpdateBaitUIEvent(int bait)
        {
            Bait = bait;
        }
    }
}