using System.Threading.Tasks;
using Auth;
using Firebase.Database;

namespace Saving
{
    public class DataBaseSaver : ISaver
    {
        private readonly IUser user;
        private const string DataBaseUrl = "https://lakelegends-ebdcd-default-rtdb.europe-west1.firebasedatabase.app/";

        public DataBaseSaver(IUser user)
        {
            this.user = user;
        }
        public async Task<string> Load(string key, string defaultValue)
        {
            var database = FirebaseDatabase.GetInstance(DataBaseUrl);
            var dataSnapshot = await database.GetReference($"{user.ID}/{key}").GetValueAsync();
            return dataSnapshot.Exists ? dataSnapshot.GetRawJsonValue() : defaultValue;
        }

        public void Save(string key, string value)
        {
            var database = FirebaseDatabase.GetInstance(DataBaseUrl);
            var dataBaseRef = database.GetReference($"{user.ID}/{key}");
            dataBaseRef.SetRawJsonValueAsync(value);
        }
    }
}