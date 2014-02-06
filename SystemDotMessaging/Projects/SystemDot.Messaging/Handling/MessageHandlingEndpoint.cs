namespace SystemDot.Messaging.Handling
{
    public class MessageHandlingEndpoint : MessageHandlerRouter, IMessageInputter<object>
    {
        public void InputMessage(object toInput)
        {
            RouteMessageToHandlers(toInput);
        }
    }
}