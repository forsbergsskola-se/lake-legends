using System;

namespace EventManagement
{
    public interface IMessageHandler
    {
        void SubscribeTo<TMessage>(Action<TMessage> callback);

        void UnsubscribeFrom<TMessage>(Action<TMessage> callback);

        void Publishh<TMessage>(TMessage message);
    }
}