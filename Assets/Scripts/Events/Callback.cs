using System;

namespace Events
{
    public class Callback
    {
        public readonly CallbackDelegate Method;
        public readonly Action CallbackMethod;

        public Callback(CallbackDelegate method, Action callbackMethod)
        {
            this.Method = method;
            this.CallbackMethod = callbackMethod;
        }
        public delegate void CallbackDelegate(Action callBack);
    }
}