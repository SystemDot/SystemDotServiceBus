using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Local.Builders;
using SystemDot.Messaging.Channels.UnitOfWork;

namespace SystemDot.Messaging.Configuration.Local
{
    public class LocalChannelConfiguration : Initialiser
    {
        readonly EndpointAddress address;
        readonly LocalChannelSchema schema;

        public LocalChannelConfiguration(
            EndpointAddress address, List<Action> buildActions) : base(buildActions)
        {
            this.address = address;
            this.schema = new LocalChannelSchema
            {
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>(),
                QueueErrors = true
            };
        }

        protected override void Build()
        {
            Resolve<LocalChannelBuilder>().Build(this.schema);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }

        public LocalChannelConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.schema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }
    }
}