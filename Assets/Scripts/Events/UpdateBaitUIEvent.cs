namespace Events
{
    public class UpdateBaitUIEvent
    {
        public readonly int Bait;
        public readonly int MaxBait;
        
        public UpdateBaitUIEvent(int bait, int maxBait)
        {
            Bait = bait;
            MaxBait = maxBait;
        }
    }
}