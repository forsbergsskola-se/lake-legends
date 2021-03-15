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
        public async Task<string> Load(string key, string defaultValue)
        {
            var database =
                FirebaseDatabase.GetInstance("https://lakelegends-ebdcd-default-rtdb.europe-west1.firebasedatabase.app/");
            var dataSnapshot = await database.GetReference($"{user.ID}/{key}").GetValueAsync();
            return dataSnapshot.Exists ? dataSnapshot.GetRawJsonValue() : defaultValue;
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
            var refe = database.GetReference($"{user.ID}/{key}");
            refe.SetRawJsonValueAsync(value);
        }
    }
}