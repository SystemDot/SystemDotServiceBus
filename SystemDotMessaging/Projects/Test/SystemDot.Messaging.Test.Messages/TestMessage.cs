using System;

namespace SystemDot.Messaging.Test.Messages
{
    [Serializable]
    public class TestMessage
    {
        public string Text { get; set; }

        public TestMessage() { }
        
        public TestMessage(string text)
        {
            Text = text;
        }
    }
}
