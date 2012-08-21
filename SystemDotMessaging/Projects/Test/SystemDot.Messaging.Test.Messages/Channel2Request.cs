namespace SystemDot.Messaging.Test.Messages
{
    public class Channel2Request : TestMessage
    {
        public Channel2Request()
        {
        }

        public Channel2Request(string text) : base(text)
        {
        }
    }

    public class Channel2Reply : TestMessage
    {
        public Channel2Reply()
        {
        }

        public Channel2Reply(string text)
            : base(text)
        {
        }
    }
}