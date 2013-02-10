using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.PointToPoint;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointReceiverConfiguration : Initialiser
    {
        readonly EndpointAddress address;

        public PointToPointReceiverConfiguration(EndpointAddress address, List<Action> buildActions) 
            : base(buildActions)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            this.address = address;
        }

        protected override void Build()
        {
            Resolve<PointToPointReceiveChannelBuilder>().Build();
            Resolve<ITransportBuilder>().Build(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }
    }
}