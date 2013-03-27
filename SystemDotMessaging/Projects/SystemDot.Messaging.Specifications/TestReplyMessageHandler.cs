namespace SystemDot.Messaging.Specifications
{
    public class TestReplyMessageHandler<T>
    {
        readonly IBus bus;

        public TestReplyMessageHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(int message)
        {
            this.bus.Reply(message);
        }
    }
}