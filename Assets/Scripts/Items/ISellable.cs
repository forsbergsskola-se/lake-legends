using System;

namespace Items
{
    public interface ISellable
    {
        void Sell();

        event Action Sold;
    }
}