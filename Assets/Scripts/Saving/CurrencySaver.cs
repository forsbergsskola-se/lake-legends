using PlayerData;

namespace Saving
{
    public class CurrencySaver : ICurrencySaver
    {
        private readonly ISaver saver;
        private readonly ISerializer serializer;

        public CurrencySaver(ISaver saver, ISerializer serializer)
        {
            this.saver = saver;
            this.serializer = serializer;
        }

        public void SaveCurrencies(string key, ICurrency currencies)
        {
            var save = serializer.SerializeObject(new CurrencySave(currencies.Silver, currencies.Gold));
            saver.Save(key, save);
        }

        public CurrencySave LoadCurrencies(string key)
        {
            var save = saver.Load(key, null);
            return serializer.DeserializeObject<CurrencySave>(save);
        }
    }
}