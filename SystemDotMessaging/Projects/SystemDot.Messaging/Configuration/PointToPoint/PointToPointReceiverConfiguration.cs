using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.PointToPoint.Builders;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointReceiverConfiguration : Initialiser
    {
        readonly ServerPath serverPath;
        readonly ChannelSchema schema;

        public PointToPointReceiverConfiguration(ServerPath serverPath, List<Action> buildActions) 
            : base(buildActions)
        {
            Contract.Requires(serverPath != null);
            this.serverPath = serverPath;
            this.schema = new ChannelSchema();
        }

        protected override void Build()
        {
            Resolve<PointToPointReceiveChannelBuilder>().Build(this.schema);
        }

        protected override ServerPath GetServerPath()
        {
            return this.serverPath;
        }

        public PointToPointReceiverConfiguration WithDurability()
        {
            this.schema.IsDurable = true;

            return this;
        }
    }
}