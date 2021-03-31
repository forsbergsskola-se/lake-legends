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
        private int bait;
        private int maxBait = 80;
        private const string CurrencyKey = "Currency";
        private IMessageHandler messageHandler;
        public Currency(ICurrencySaver saver, IMessageHandler messageHandler)
        {
            this.saver = saver;
            this.messageHandler = messageHandler;
            this.messageHandler.SubscribeTo<IncreaseSilverEvent>(silverEvent => Silver += silverEvent.Silver);
            this.messageHandler.SubscribeTo<DecreaseSilverEvent>(silverEvent => Silver -= silverEvent.Silver);
            this.messageHandler.SubscribeTo<IncreaseGoldEvent>(goldEvent => Gold += goldEvent.Gold);
            this.messageHandler.SubscribeTo<DecreaseGoldEvent>(goldEvent => Gold -= goldEvent.Gold);
            this.messageHandler.SubscribeTo<IncreaseBaitEvent>(baitEvent =>
            {
                if (baitEvent.IsPremium)
                    Bait += baitEvent.Bait;
                else
                    AddRegenBait(baitEvent.Bait);
            });
            this.messageHandler.SubscribeTo<DecreaseBaitEvent>(baitEvent => Bait -= baitEvent.Bait);
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

        private void AddRegenBait(int amount)
        {
            Bait += Mathf.Clamp(amount, 0, Mathf.Clamp(MaxBait - Bait, 0, maxBait));
        }

        public int Bait
        {
            get => bait;
            private set
            {
                bait = Mathf.Clamp(value, 0, int.MaxValue);
                messageHandler.Publish(new UpdateBaitUIEvent(bait, MaxBait));
                Serialize();
            }
        }

        public int MaxBait => maxBait;

        public void Serialize()
        {
            saver.SaveCurrencies(CurrencyKey, this);
        }

        public async Task Deserialize()
        {
            var currencies = await saver.LoadCurrencies(CurrencyKey);
            silver = currencies.Silver;
            gold = currencies.Gold;
            bait = currencies.Bait;
            messageHandler.Publish(new UpdateSilverUIEvent(silver));
            messageHandler.Publish(new UpdateGoldUIEvent(gold));
        }
    }
}