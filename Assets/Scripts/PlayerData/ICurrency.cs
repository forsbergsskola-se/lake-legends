using System.Threading.Tasks;

namespace PlayerData
{
    public interface ICurrency
    {
        int Silver { get; }
        int Gold { get; }
        int Bait { get; }
        int MaxBait { get; }
        void Serialize();
        Task Deserialize();
    }
}