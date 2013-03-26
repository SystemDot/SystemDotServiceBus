namespace SystemDot.Messaging.Specifications.channels.handling.Fakes
{
    class SecondHandlerOfMessage2 : IHandleMessage
    {
        public Message2 LastHandledMessage { get; private set; }

        public void Handle(Message2 message)
        {
            LastHandledMessage = message;
        }
    }
}