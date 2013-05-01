using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointReceiverConfiguration : Configurer
    {
        readonly ServerPath serverPath;
        readonly PointToPointReceiverChannelSchema schema;

        public PointToPointReceiverConfiguration(EndpointAddress address, ServerPath serverPath, List<Action> buildActions) 
            : base(buildActions)
        {
            Contract.Requires(address != null);
            Contract.Requires(serverPath != null);
            Contract.Requires(buildActions != null);

            this.serverPath = serverPath;

            this.schema = new PointToPointReceiverChannelSchema
            {
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>,
                Address = address
            };
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

        public PointToPointReceiverConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.schema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }
    }
}