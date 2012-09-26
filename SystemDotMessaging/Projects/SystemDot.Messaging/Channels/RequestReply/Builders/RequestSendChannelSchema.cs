using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Filtering;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestSendChannelSchema
    {
        public IMessageFilterStrategy FilteringStrategy { get; private set; }

        public EndpointAddress FromAddress { get; private set; }

        public EndpointAddress RecieverAddress { get; private set; }

        public RequestSendChannelSchema(
            IMessageFilterStrategy filteringStrategy, 
            EndpointAddress fromAddress, 
            EndpointAddress recieverAddress)
        {
            this.FilteringStrategy = filteringStrategy;
            this.FromAddress = fromAddress;
            this.RecieverAddress = recieverAddress;

            Contract.Requires(filteringStrategy != null);
            Contract.Requires(fromAddress != null);
            Contract.Requires(fromAddress != EndpointAddress.Empty);
            Contract.Requires(recieverAddress != null);
            Contract.Requires(recieverAddress != EndpointAddress.Empty);
        }
    }
}