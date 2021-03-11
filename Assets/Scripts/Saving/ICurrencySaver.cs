using PlayerData;

namespace Saving
{
    public interface ICurrencySaver
    {
        void SaveCurrencies(string key, ICurrency currencies);
        CurrencySave LoadCurrencies(string key);
    }
}