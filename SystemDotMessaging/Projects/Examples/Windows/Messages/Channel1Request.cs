namespace Messages
{
    public class Channel1Request : TestMessage
    {
        public Channel1Request()
        {
        }

        public Channel1Request(string text) : base(text)
        {
        }
    }

    public class Channel1Reply : TestMessage
    {
        public Channel1Reply()
        {
        }

        public Channel1Reply(string text)
            : base(text)
        {
        }
    }
}