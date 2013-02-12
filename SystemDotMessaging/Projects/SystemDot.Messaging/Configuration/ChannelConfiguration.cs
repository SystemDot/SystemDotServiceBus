using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.PointToPoint;
using SystemDot.Messaging.Configuration.Publishers;
using SystemDot.Messaging.Configuration.RequestReply;

namespace SystemDot.Messaging.Configuration
{
    public class ChannelConfiguration : Configurer
    {
        readonly EndpointAddress address;
        readonly ServerPath serverPath;
        readonly List<Action> buildActions;

        public ChannelConfiguration(EndpointAddress address, ServerPath serverPath, List<Action> buildActions)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(serverPath != null);
            Contract.Requires(buildActions != null);

            this.address = address;
            this.serverPath = serverPath;
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
                BuildEndpointAddress(recieverAddress, this.serverPath),
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
                BuildEndpointAddress(publisherAddress, this.serverPath),
                this.buildActions);
        }

        public PointToPointSenderConfiguration ForPointToPointSendingTo(string recieverAddress)
        {
            return new PointToPointSenderConfiguration(
                this.address,
                BuildEndpointAddress(recieverAddress, this.serverPath), 
                this.buildActions);
        }

        public PointToPointReceiverConfiguration ForPointToPointReceiving()
        {
            return new PointToPointReceiverConfiguration(this.serverPath, this.buildActions);
        }
    }

    
}