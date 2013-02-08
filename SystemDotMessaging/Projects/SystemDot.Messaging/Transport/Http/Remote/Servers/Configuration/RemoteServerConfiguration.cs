using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    public class RemoteServerConfiguration : Initialiser
    {
        public RemoteServerConfiguration(List<Action> buildActions) : base(buildActions)
        {
        }

        protected override void Build()
        {
            Resolve<ITransportBuilder>().Build();
        }

        protected override EndpointAddress GetAddress()
        {
            throw new NotImplementedException();
        }
    }
}