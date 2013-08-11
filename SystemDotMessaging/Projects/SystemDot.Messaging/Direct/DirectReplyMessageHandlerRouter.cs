using SystemDot.Messaging.Handling;

namespace SystemDot.Messaging.Direct
{
    class DirectReplyMessageHandlerRouter : BasicMessageHandlerRouter
    {
        protected override void RouteMessageToHandlers(object message)
        {
            RegisterHandler(DirectSendContext.GetHandleReplyWith());
            base.RouteMessageToHandlers(message);
            UnregisterHandler(DirectSendContext.GetHandleReplyWith());
        }
    }
}