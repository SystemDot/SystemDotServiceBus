using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Direct
{
    class DirectReplyContext : Disposable
    {
        readonly static ThreadLocal<ContextData> current = new ThreadLocal<ContextData>();

        public DirectReplyContext(EndpointAddress address, EndpointAddress clientAddress)
        {
            current.Value = new ContextData(address, clientAddress);
        }

        public static EndpointAddress GetCurrentAddress()
        {
            return current.Value.Address;
        }

        public static EndpointAddress GetCurrentClientAddress()
        {
            return current.Value.ClientAddress;
        }

        public IEnumerable<MessagePayload> GetCurrentReplies()
        {
            return current.Value.Replies;
        }

        public static void AddReply(MessagePayload toAdd)
        {
            Contract.Requires(toAdd != null);

            current.Value.Replies.Add(toAdd);
        }

        protected override void DisposeOfManagedResources()
        {
            current.Value = null;
            base.DisposeOfManagedResources();
        }

        class ContextData
        {
            public EndpointAddress Address { get; private set; }

            public EndpointAddress ClientAddress { get; private set; }

            public List<MessagePayload> Replies { get; private set; }

            public ContextData(EndpointAddress address, EndpointAddress clientAddress)
            {
                Address = address;
                ClientAddress = clientAddress;
                Replies = new List<MessagePayload>();
            }
        }
    }
}