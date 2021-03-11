namespace Events
{
    public class UpdateSilverUIEvent
    {
        public readonly int Silver;

        public UpdateSilverUIEvent(int silver)
        {
            Silver = silver;
        }
    }
}