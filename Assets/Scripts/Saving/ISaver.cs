using System.Threading.Tasks;

namespace Saving
{
    public interface ISaver
    {
        Task<string> Load(string key, string defaultValue);
        void Save(string key, string value);
    }
}