using System;
using System.Threading.Tasks;
using Auth;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

namespace Saving
{
    public class DataBaseSaver : ISaver
    {
        private readonly IUser user;

        public DataBaseSaver(IUser user)
        {
            this.user = user;
        }
        public string Load(string key, string defaultValue)
        {
            var database =
                FirebaseDatabase.GetInstance("https://lakelegends-ebdcd-default-rtdb.europe-west1.firebasedatabase.app/");
            var reference = database.GetReference($"{user.ID}{key}");
            var refe = database.GetReference($"{user.ID}{key}");
            var value = refe.GetValueAsync();
            return value.Result.Value as string;
        }

        private async Task<string> LoadAsync(DatabaseReference reference)
        {
            var task = await Task.Run(reference.GetValueAsync);
            return task.GetRawJsonValue();
        }

        public void Save(string key, string value)
        {
            var database =
                FirebaseDatabase.GetInstance(
                    "https://lakelegends-ebdcd-default-rtdb.europe-west1.firebasedatabase.app/");
            var refe = database.GetReference($"{user.ID}{key}");
            refe.SetRawJsonValueAsync(value);
        }
    }
}