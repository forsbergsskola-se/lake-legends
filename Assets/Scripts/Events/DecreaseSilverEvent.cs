namespace Events
{
    public class DecreaseSilverEvent
    {
        public readonly int Silver;

        public DecreaseSilverEvent(int silver)
        {
            Silver = silver;
        }
    }
}