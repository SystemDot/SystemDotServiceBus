using System.Collections.Generic;
using SystemDot.Messaging.Channels.Sending.Http;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    public class TestMessageWebRequestor : IMessageWebRequestor 
    {
        public TestMessageWebRequestor()
        {
            this.SentMessages = new List<object>();
        }

        public void PutMessage(object message)
        {
            this.SentMessages.Add(message);
        }

        public List<object> SentMessages { get; private set; }
    }
}