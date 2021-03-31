namespace Events
{
    public class IncreaseBaitEvent
    {
        public int Bait;
        public readonly bool IsPremium;

        public IncreaseBaitEvent(int bait, bool isPremium)
        {
            Bait = bait;
            IsPremium = isPremium;
        }
    }
}