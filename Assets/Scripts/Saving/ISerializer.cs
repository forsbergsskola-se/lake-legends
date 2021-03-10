namespace Saving
{
    public interface ISerializer
    {
        string SerializeObject(object obj);
        T DeserializeObject<T>(string str);
    }
}