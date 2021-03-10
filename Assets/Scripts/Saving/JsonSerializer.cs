using Newtonsoft.Json;

namespace Saving
{
    public class JsonSerializer : ISerializer
    {
        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T DeserializeObject<T>(string str) => JsonConvert.DeserializeObject<T>(str);
    }
}