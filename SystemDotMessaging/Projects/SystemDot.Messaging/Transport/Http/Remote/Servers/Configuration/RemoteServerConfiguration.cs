using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    public class RemoteServerConfiguration : Initialiser
    {
        public RemoteServerConfiguration(List<Action> actions) : base(actions)
        {
        }

        protected override void Build()
        {
            Resolve<ITransportBuilder>().Build(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return new EndpointAddress();
        }
    }
}