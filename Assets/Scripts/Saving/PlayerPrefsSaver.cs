using System.Threading.Tasks;
using UnityEngine;

namespace Saving
{
    public class PlayerPrefsSaver : ISaver
    {
        public async Task<string> Load(string key, string defaultValue)
        {
            var str = await GetKey(key, defaultValue);
            return str;
        }

        public void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        private async Task<string> GetKey(string key, string defaultValue)
        {
            return await Task.FromResult(PlayerPrefs.GetString(key, defaultValue));
        }
    }
}