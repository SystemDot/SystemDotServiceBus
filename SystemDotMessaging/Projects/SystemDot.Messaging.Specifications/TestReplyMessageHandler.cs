namespace SystemDot.Messaging.Specifications
{
    public class TestReplyMessageHandler<TRequest>
    {
        public void Handle(TRequest message)
        {
            Bus.Reply(message);
        }
    }

    public class TestReplyMessageHandler<TRequest, TResponse> where TResponse : new()
    {
        public void Handle(TRequest message)
        {
            Bus.Reply(new TResponse());
        }
    }
}