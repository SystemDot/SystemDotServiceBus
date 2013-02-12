using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.PointToPoint;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointReceiverConfiguration : Initialiser
    {
        readonly ServerPath serverPath;

        public PointToPointReceiverConfiguration(ServerPath serverPath, List<Action> buildActions) 
            : base(buildActions)
        {
            Contract.Requires(serverPath != null);
            this.serverPath = serverPath;
        }

        protected override void Build()
        {
            Resolve<PointToPointReceiveChannelBuilder>().Build();
        }

        protected override ServerPath GetServerPath()
        {
            return this.serverPath;
        }
    }
}