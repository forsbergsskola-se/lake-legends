using System.Threading.Tasks;
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
            this.messageHandler.SubscribeTo<DecreaseSilverEvent>(silverEvent => Silver -= silverEvent.Silver);
            this.messageHandler.SubscribeTo<DecreaseGoldEvent>(goldEvent => Gold -= goldEvent.Gold);
        }

        public int Silver
        {
            get => silver;
            private set 
            { 
                silver = value;
                messageHandler.Publish(new UpdateSilverUIEvent(silver));
                Serialize();
            }
        }

        public int Gold
        {
            get => gold;
            private set
            { 
                gold = value;
                messageHandler.Publish(new UpdateGoldUIEvent(gold));
                Serialize();
            }
        }

        public void Serialize()
        {
            saver.SaveCurrencies(CurrencyKey, this);
        }

        public async Task Deserialize()
        {
            var currencies = await saver.LoadCurrencies(CurrencyKey);
            silver = currencies.Silver;
            gold = currencies.Gold;
            messageHandler.Publish(new UpdateSilverUIEvent(silver));
            messageHandler.Publish(new UpdateGoldUIEvent(gold));
        }
    }
}