namespace Events
{
    public class UpdateCaptureZoneUISizeEvent
    {
        public float percentage;
        public float width;

        public UpdateCaptureZoneUISizeEvent(float percentage, float width)
        {
            this.percentage = percentage;
            this.width = width;
        }
    }
}