using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.Http;
using SystemDot.Messaging.Transport.InProcess;

namespace SystemDot.Messaging.Specifications
{
    public class TestInProcessMessageServer : IInProcessMessageServer
    {
        readonly IMessagingServerHandler[] handlers;

        public List<MessagePayload> SentMessages { get; private set; }
        public List<MessagePayload> ReturnedMessages { get; private set; }

        public TestInProcessMessageServer(params IMessagingServerHandler[] handlers)
        {
            this.handlers = handlers;
            SentMessages = new List<MessagePayload>();
            ReturnedMessages = new List<MessagePayload>();
        }

        public List<MessagePayload> InputMessage(MessagePayload toInput)
        {
            SentMessages.Add(toInput);
            return ReturnedMessages;
        }

        public void ReceiveMessage(MessagePayload toInput)
        {
            handlers.ForEach(h => h.HandleMessage(toInput, ReturnedMessages));
        }

        public void ReplyAfterRequestSentWith(MessagePayload toReplyWith) 
        { 
            ReturnedMessages.Add(toReplyWith);
        }

        public MessagePayload GetOnlyMessage()
        {
            return SentMessages.Single();
        }

        public void ReceiveAcknowledgementFor(MessagePayload messagePayload)
        {
            ReceiveMessage(
                new MessagePayload()
                    .AsAcknowlegement(messagePayload.GetPersistenceId())
                    .SetToChannel(messagePayload.GetFromAddress().Channel));
        }
    }
}