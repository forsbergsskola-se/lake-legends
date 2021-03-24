namespace Events
{
    public class DecreaseBaitEvent
    {
        public int Bait;

        public DecreaseBaitEvent(int bait) => Bait = bait;
    }
}