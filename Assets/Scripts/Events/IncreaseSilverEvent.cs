namespace Events
{
    public class IncreaseSilverEvent
    {
        public readonly int Silver;

        public IncreaseSilverEvent(int silver)
        {
            Silver = silver;
        }
    }
}