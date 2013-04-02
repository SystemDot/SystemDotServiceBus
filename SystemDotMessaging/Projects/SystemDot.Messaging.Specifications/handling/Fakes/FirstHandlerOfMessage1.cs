namespace SystemDot.Messaging.Specifications.handling.Fakes
{
    class FirstHandlerOfMessage1 : IHandleMessage
    {
        public Message1 LastHandledMessage { get; private set; }

        public void Handle(Message1 message)
        {
            this.LastHandledMessage = message;
        }
    }
}