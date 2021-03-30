namespace Events
{
    public class AnimationBlendEvent
    {
        public string blendName;
        public float blendValue;

        public AnimationBlendEvent(string blendName, float blendValue)
        {
            this.blendName = blendName;
            this.blendValue = blendValue;
        }
    }
}