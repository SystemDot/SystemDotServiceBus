using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Configuration.Publishers;
using SystemDot.Messaging.Configuration.RequestReply;

namespace SystemDot.Messaging.Configuration
{
    public class ChannelConfiguration : Configurer
    {
        readonly EndpointAddress address;
        readonly string messageServerName;
        readonly List<Action> buildActions;

        public ChannelConfiguration(EndpointAddress address, string messageServerName, List<Action> buildActions)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(!String.IsNullOrEmpty(messageServerName));
            Contract.Requires(buildActions != null);

            this.address = address;
            this.messageServerName = messageServerName;
            this.buildActions = buildActions;
        }

        public RequestReplyRecieverConfiguration ForRequestReplyRecieving()
        {
            return new RequestReplyRecieverConfiguration(address, this.buildActions);
        }

        public RequestReplySenderConfiguration ForRequestReplySendingTo(string recieverAddress)
        {
            return new RequestReplySenderConfiguration(
                this.address,
                BuildEndpointAddress(recieverAddress, this.messageServerName),
                this.buildActions);
        }

        public PublisherConfiguration ForPublishing()
        {
            return new PublisherConfiguration(address, this.buildActions);
        }

        public SubscribeToConfiguration ForSubscribingTo(string publisherAddress)
        {
            return new SubscribeToConfiguration(
                this.address, 
                BuildEndpointAddress(publisherAddress, this.messageServerName),
                this.buildActions);
        }
    }
}