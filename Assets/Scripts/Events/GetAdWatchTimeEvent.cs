using System;

namespace Events
{
    public class GetAdWatchTimeEvent
    {
        public readonly DateTime latestAdWatchTime;

        public GetAdWatchTimeEvent(DateTime latestAdWatchTime)
        {
            this.latestAdWatchTime = latestAdWatchTime;
        }
    }
}