using SystemDot.Messaging.Handling;
using SystemDot.ThreadMashalling;

namespace SystemDot.Messaging.Direct
{
    class DirectReplyMessageHandlerRouter : IMessageInputter<object>
    {
        readonly IMainThreadMarshaller marshaller;
        readonly MessageHandlerRouter inner;

        public DirectReplyMessageHandlerRouter(IMainThreadMarshaller marshaller, MessageHandlerRouter inner)
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