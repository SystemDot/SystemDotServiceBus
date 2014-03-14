using SystemDot.Messaging.Hooks.External;

namespace SystemDot.Messaging.Specifications.external_hooks
{
    class TestExternalHook<T> : ExternalHook<T>
    {
        public T Message { get; private set; }

        protected override T ProcessMessage(T toInput)
        {
            Message = toInput;
            return toInput;
        }
    }
}