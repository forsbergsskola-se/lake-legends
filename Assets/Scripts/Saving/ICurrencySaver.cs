using System.Threading.Tasks;
using PlayerData;

namespace Saving
{
    public interface ICurrencySaver
    {
        void SaveCurrencies(string key, ICurrency currencies);
        Task<CurrencySave> LoadCurrencies(string key);
    }
}