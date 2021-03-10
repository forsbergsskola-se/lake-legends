using UnityEngine;

namespace Saving
{
    public class PlayerPrefsSaver : ISaver
    {
        public string Load(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}