namespace SystemDot.Messaging.Specifications.handling.Fakes
{
    class FirstHandlerOfMessage2 : IHandleMessage
    {
        public Message2 LastHandledMessage { get; private set; }

        public void Handle(Message2 message)
        {
            this.LastHandledMessage = message;
        }
    }
}