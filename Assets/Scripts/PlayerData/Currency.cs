using EventManagement;
using Events;
using Saving;
using UnityEngine;

namespace PlayerData
{
    public class Currency : ICurrency
    {
        private readonly ICurrencySaver saver;
        private int silver;
        private int gold;
        private const string CurrencyKey = "Currency";
        private IMessageHandler messageHandler;
        public Currency(ICurrencySaver saver, IMessageHandler messageHandler)
        {
            this.saver = saver;
            this.messageHandler = messageHandler;
            this.messageHandler.SubscribeTo<IncreaseSilverEvent>(silverEvent => Silver += silverEvent.Silver);
            this.messageHandler.SubscribeTo<IncreaseGoldEvent>(goldEvent => Gold += goldEvent.Gold);
        }

        public int Silver
        {
            get => silver;
            private set 
            { 
                silver = value; 
                Debug.Log($"Current Silver {silver}");
                messageHandler.Publish(new UpdateSilverUIEvent(silver));
            }
        }

        public int Gold
        {
            get => gold;
            private set => gold = value;
        }

        public void Serialize()
        {
            saver.SaveCurrencies(CurrencyKey, this);
        }

        public void Deserialize()
        {
            var currencies = saver.LoadCurrencies(CurrencyKey) ?? new CurrencySave(0 ,0);
            Silver = currencies.Silver;
            Gold = currencies.Gold;
        }
    }
}