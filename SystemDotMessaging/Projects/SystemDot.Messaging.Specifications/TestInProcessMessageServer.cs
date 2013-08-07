using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport.Http;
using SystemDot.Messaging.Transport.InProcess;

namespace SystemDot.Messaging.Specifications
{
    public class TestInProcessMessageServer : IInProcessMessageServer
    {
        readonly IMessagingServerHandler[] handlers;
        public IList<MessagePayload> SentMessages { get; private set; }

        public TestInProcessMessageServer(params IMessagingServerHandler[] handlers)
        {
            this.handlers = handlers;
            SentMessages = new List<MessagePayload>();
        }

        public List<MessagePayload> InputMessage(MessagePayload toInput)
        {
            SentMessages.Add(toInput);
            return new List<MessagePayload>();
        }

        public void ReceiveMessage(MessagePayload toInput)
        {
            var outgoingMessages = new List<MessagePayload>();
            handlers.ForEach(h => h.HandleMessage(toInput, outgoingMessages));
        }
    }
}