using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine;

namespace Saving
{
    public class PlayerPrefsSaver : ISaver
    {
        public async Task<string> Load(string key, string defaultValue)
        {
            var str = GetKey(key, defaultValue);
            return str;
        }

        public void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        private string GetKey(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
    }
}