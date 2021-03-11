namespace Events
{
    public class IncreaseGoldEvent
    {
        public readonly int Gold;

        public IncreaseGoldEvent(int gold)
        {
            Gold = gold;
        }
    }
}