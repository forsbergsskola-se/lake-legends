using System.Threading.Tasks;

namespace PlayerData
{
    public interface ICurrency
    {
        int Silver { get; }
        int Gold { get; }
        void Serialize();
        Task Deserialize();
    }
}