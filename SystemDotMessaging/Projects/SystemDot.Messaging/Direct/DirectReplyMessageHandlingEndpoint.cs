using SystemDot.Messaging.Handling;
using SystemDot.ThreadMarshalling;

namespace SystemDot.Messaging.Direct
{
    class DirectReplyMessageHandlingEndpoint : IMessageInputter<object>
    {
        readonly IMainThreadMarshaller marshaller;
        readonly MessageHandlingEndpoint inner;

        public DirectReplyMessageHandlingEndpoint(IMainThreadMarshaller marshaller, MessageHandlingEndpoint inner)
        {
            this.marshaller = marshaller;
            this.inner = inner;
        }


        public void InputMessage(object toInput)
        {
            if (HasDirectSendContextHandler())
                RouteMessageWithDirectSendContextHandler(toInput);
            else
                RouteMessageOnMainThread(toInput);
        }

        static bool HasDirectSendContextHandler()
        {
            return DirectSendContext.HasReplyHandler();
        }

        void RouteMessageWithDirectSendContextHandler(object toInput)
        {
            inner.RegisterHandler(DirectSendContext.GetHandleReplyWith());

            RouteMessageOnMainThread(toInput);

            inner.UnregisterHandler(DirectSendContext.GetHandleReplyWith());
        }

        void RouteMessageOnMainThread(object toInput)
        {
            marshaller.RunOnMainThread(() => inner.InputMessage(toInput));
        }
    }
}