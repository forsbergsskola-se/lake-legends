namespace Events
{
    public class IncreaseBaitEvent
    {
        public int Bait;

        public IncreaseBaitEvent(int bait) => Bait = bait;
    }
}