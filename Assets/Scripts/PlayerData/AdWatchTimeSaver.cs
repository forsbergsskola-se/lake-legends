using System;
using System.Threading.Tasks;
using EventManagement;
using Events;
using Saving;

namespace PlayerData
{
    public class AdWatchTimeSaver
    {
        private DateTime LatestAdWatch {get;set;}
        
        private readonly ISaver saver;
        private readonly ISerializer serializer;
        private readonly IMessageHandler messageHandler;

        private const string Key = "LatestAdWatch";
        public AdWatchTimeSaver(ISaver saver, ISerializer serializer, IMessageHandler messageHandler)
        {
            this.saver = saver;
            this.serializer = serializer;
            this.messageHandler = messageHandler;
            messageHandler.SubscribeTo<SaveAdWatchTimeEvent>(OnWatchAd);
            messageHandler.SubscribeTo<RequestAdWatchTimeEvent>(OnAdWatchRequest);
        }

        private void OnAdWatchRequest(RequestAdWatchTimeEvent obj)
        {
            messageHandler.Publish(new GetAdWatchTimeEvent(LatestAdWatch));
        }

        private void OnWatchAd(SaveAdWatchTimeEvent obj)
        {
            LatestAdWatch = DateTime.UtcNow;
                Save();
        }

        public async Task Load()
        {
            var savefile = await saver.Load(Key, serializer.SerializeObject(DateTime.MinValue));
            LatestAdWatch = serializer.DeserializeObject<DateTime>(savefile);
        }

        public void Save()
        {
            var serializedObject = serializer.SerializeObject(LatestAdWatch);
            saver.Save(Key, serializedObject);
        }
    }
}