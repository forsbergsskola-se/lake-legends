using System.Threading.Tasks;
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
            var save = serializer.SerializeObject(new CurrencySave(currencies.Silver, currencies.Gold, currencies.Bait));
            saver.Save(key, save);
        }

        public async Task<CurrencySave> LoadCurrencies(string key)
        {
            var save = await saver.Load(key, serializer.SerializeObject(new CurrencySave(0, 0, 200)));
            return serializer.DeserializeObject<CurrencySave>(save);
        }
    }
}