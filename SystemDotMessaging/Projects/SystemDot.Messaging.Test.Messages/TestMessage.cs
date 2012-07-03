using System;

namespace SystemDot.Messaging.Test.Messages
{
    [Serializable]
    public class TestMessage
    {
        public string Text { get; private set; }

        public TestMessage(string text)
        {
            Text = text;
        }
    }
}
