using System;
using System.Threading.Tasks;

namespace Saving
{
    public class SinceLastLoginSaver
    {
        private readonly ISaver saver;
        private readonly ISerializer serializer;
        private const string Key = "LastLogin";
        public SinceLastLoginSaver(ISerializer serializer, ISaver saver)
        {
            this.serializer = serializer;
            this.saver = saver;
        }

        public void Save()
        {
            var currentTime = serializer.SerializeObject(DateTime.UtcNow);
            saver.Save(Key, currentTime);
        }

        public async Task<int> GetTimeSinceLastLogin()
        {
            var save = await saver.Load(Key, serializer.SerializeObject(DateTime.UtcNow));
            var lastLogin = serializer.DeserializeObject<DateTime>(save);
            return (int)(DateTime.UtcNow - lastLogin).TotalSeconds;
        }
    }
}