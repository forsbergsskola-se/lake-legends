using EventManagement;
using Events;
using Saving;

namespace PlayerData
{
    public class Currency : ICurrency
    {
        private readonly ICurrencySaver saver;
        private const string CurrencyKey = "Currency";
        public Currency(ICurrencySaver saver, IMessageHandler messageHandler)
        {
            this.saver = saver;
            messageHandler.SubscribeTo<IncreaseSilverEvent>(silverEvent => Silver += silverEvent.Silver);
            messageHandler.SubscribeTo<IncreaseGoldEvent>(goldEvent => Gold += goldEvent.Gold);
        }
        public int Silver { get; private set; }
        public int Gold { get; private set; }
        public void Serialize()
        {
            saver.SaveCurrencies(CurrencyKey, this);
        }

        public void Deserialize()
        {
            var currencies = saver.LoadCurrencies(CurrencyKey);
            Silver = currencies.Silver;
            Gold = currencies.Gold;
        }
    }
}