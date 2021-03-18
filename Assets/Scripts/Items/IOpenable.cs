using System;

namespace Items
{
    public interface IOpenable
    {
        void Open(Action openListener);
    }
}