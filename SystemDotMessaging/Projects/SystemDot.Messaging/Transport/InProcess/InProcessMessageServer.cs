using System.Collections.Generic;
using SystemDot.Core.Collections;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport.Http;

namespace SystemDot.Messaging.Transport.InProcess
{
    class InProcessMessageServer : IInProcessMessageServer
    {
        readonly IMessagingServerHandler[] handlers;

        public InProcessMessageServer(params IMessagingServerHandler[] handlers)
        {
            this.handlers = handlers;
        }

        public List<MessagePayload> InputMessage(MessagePayload toInput)
        {
            var outgoingMessages = new List<MessagePayload>();

            handlers.ForEach(h => h.HandleMessage(toInput, outgoingMessages));

            return outgoingMessages;
        }
    }
}