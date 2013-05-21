using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.PointToPoint;
using SystemDot.Messaging.Configuration.Publishers;
using SystemDot.Messaging.Configuration.RequestReply;

namespace SystemDot.Messaging.Configuration
{
    public class ChannelConfiguration : ConfigurationBase
    {
        readonly EndpointAddress address;
        readonly ServerPath serverPath;
        readonly MessagingConfiguration messagingConfiguration;

        public ChannelConfiguration(EndpointAddress address, ServerPath serverPath, MessagingConfiguration messagingConfiguration)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(serverPath != null);
            Contract.Requires(messagingConfiguration != null);

            this.address = address;
            this.serverPath = serverPath;
            this.messagingConfiguration = messagingConfiguration;
        }

        public RequestReplyRecieverConfiguration ForRequestReplyRecieving()
        {
            return new RequestReplyRecieverConfiguration(address, this.messagingConfiguration);
        }

        public RequestReplySenderConfiguration ForRequestReplySendingTo(string recieverAddress)
        {
            return new RequestReplySenderConfiguration(
                this.address,
                BuildEndpointAddress(recieverAddress, this.serverPath),
                this.messagingConfiguration);
        }

        public PublisherConfiguration ForPublishing()
        {
            return new PublisherConfiguration(address, this.messagingConfiguration);
        }

        public SubscribeToConfiguration ForSubscribingTo(string publisherAddress)
        {
            return new SubscribeToConfiguration(
                this.address, 
                BuildEndpointAddress(publisherAddress, this.serverPath),
                this.messagingConfiguration);
        }

        public PointToPointSenderConfiguration ForPointToPointSendingTo(string recieverAddress)
        {
            return new PointToPointSenderConfiguration(
                this.address,
                BuildEndpointAddress(recieverAddress, this.serverPath),
                this.messagingConfiguration);
        }

        public PointToPointReceiverConfiguration ForPointToPointReceiving()
        {
            return new PointToPointReceiverConfiguration(this.address, this.serverPath, this.messagingConfiguration);
        }
    }

    
}