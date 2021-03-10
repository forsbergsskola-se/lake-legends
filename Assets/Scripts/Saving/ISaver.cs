namespace Saving
{
    public interface ISaver
    {
        string Load(string key, string defaultValue);
        void Save(string key, string value);
    }
}