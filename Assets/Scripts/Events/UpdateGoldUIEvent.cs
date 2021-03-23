namespace Events
{
    public class UpdateGoldUIEvent
    {
        public readonly int Gold;

        public UpdateGoldUIEvent(int gold)
        {
            Gold = gold;
        }
    }
}