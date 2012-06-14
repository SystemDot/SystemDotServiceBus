using System.Collections.Generic;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Servers;

namespace SystemDot.Messaging.Specifications.message_serving
{
    public class TestMessagingServerHandler : IMessagingServerHandler
    {
        private readonly MessagePayload[] messagesToReplyWith;

        public bool HandlesMessages { get; set; }

        public MessagePayload HandledPayload { get; private set; }

        public TestMessagingServerHandler(params MessagePayload[] messagesToReplyWith)
        {
            this.messagesToReplyWith = messagesToReplyWith;
            this.HandlesMessages = true;
        }

        public bool ShouldHandleMessage(MessagePayload toCheck)
        {
            return HandlesMessages;
        }

        public void HandleMessage(MessagePayload toHandle)
        {
            this.HandledPayload = toHandle;
        }

        public IEnumerable<MessagePayload> Reply()
        {
            return this.messagesToReplyWith;
        }

        
    }
}