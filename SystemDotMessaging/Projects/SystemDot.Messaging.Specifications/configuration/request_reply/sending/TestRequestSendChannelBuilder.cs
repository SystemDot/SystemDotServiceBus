using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    public class TestRequestSendChannelBuilder : IRequestSendChannelBuilder
    {
        public EndpointAddress From { get; private set; }
        
        public EndpointAddress Reciever { get; private set; }

        public IMessageFilterStrategy MessageFilter{ get; private set; }

        public void Build(IMessageFilterStrategy filteringStrategy, EndpointAddress fromAddress, EndpointAddress recieverAddress)
        {
            MessageFilter = filteringStrategy;
            From = fromAddress;
            Reciever = recieverAddress;
        }
    }
}