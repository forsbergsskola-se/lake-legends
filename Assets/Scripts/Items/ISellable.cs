using System;

namespace Items
{
    public interface ISellable
    {
        void Sell();

        int Value { get; }

        event Action Sold;
    }
}