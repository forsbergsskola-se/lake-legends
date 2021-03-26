namespace Events
{
    public class TimeSinceLastLoginEvent
    {
        public readonly int Seconds;

        public TimeSinceLastLoginEvent(int seconds) => Seconds = seconds;
    }
}