using System;

namespace SystemDot.Messaging.Hooks.External
{
    public abstract class ExternalHook<T> : IExternalHook
    {
        public Type MessageType
        {
            get { return typeof (T); }
        }

        public object ProcessMessage(object toInput)
        {
            return ProcessMessage(toInput.As<T>());
        }

        protected abstract T ProcessMessage(T toInput);
    }
}