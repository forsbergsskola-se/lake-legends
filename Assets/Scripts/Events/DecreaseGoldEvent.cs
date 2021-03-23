namespace Events
{
    public class DecreaseGoldEvent
    {
        public readonly int Gold;

        public DecreaseGoldEvent(int gold)
        {
            Gold = gold;
        }
    }
}