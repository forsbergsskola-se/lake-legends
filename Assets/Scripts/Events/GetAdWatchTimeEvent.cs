using System;

namespace Events
{
    public class GetAdWatchTimeEvent
    {
        public readonly DateTime LatestAdWatchTime;

        public GetAdWatchTimeEvent(DateTime latestAdWatchTime)
        {
            LatestAdWatchTime = latestAdWatchTime;
        }
    }
}