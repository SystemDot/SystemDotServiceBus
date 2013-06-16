namespace SystemDot.Messaging.Specifications
{
    public class TestReplyMessageHandler<T>
    {
        public void Handle(T message)
        {
            Bus.Reply(message);
        }
    }
}