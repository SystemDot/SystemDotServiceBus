namespace SystemDot.Messaging.Specifications
{
    public class TestReplyMessageHandler<T>
    {
        public void Handle(int message)
        {
            Bus.Reply(message);
        }
    }
}