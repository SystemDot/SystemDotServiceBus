using System.Collections.Generic;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageSender : IMessageSender
    {
        public TestMessageSender()
        {
            SentMessages = new List<MessagePayload>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            SentMessages.Add(toInput);
        }

        public IList<MessagePayload> SentMessages { get; private set; }
    }
}