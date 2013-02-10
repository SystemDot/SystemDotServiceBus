using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport.InProcess;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageServer : MessageProcessor, IInProcessMessageServer
    {
        public IList<MessagePayload> SentMessages { get; private set; }

        public TestMessageServer()
        {
            SentMessages = new List<MessagePayload>();
        }

        public override void InputMessage(MessagePayload toInput)
        {
            SentMessages.Add(toInput);
        }

        public void ReceiveMessage(MessagePayload toInput)
        {
            OnMessageProcessed(toInput);
        }
    }
}