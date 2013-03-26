namespace SystemDot.Messaging.Specifications.channels.handling.Fakes
{
    class SecondHandlerOfMessage1 : IHandleMessage
    {
        public Message1 LastHandledMessage { get; private set; }

        public void Handle(Message1 message)
        {
            LastHandledMessage = message;
        }
    }
}