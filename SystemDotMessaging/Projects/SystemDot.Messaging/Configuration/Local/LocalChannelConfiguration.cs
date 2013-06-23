using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Local.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.Local
{
    public class LocalChannelConfiguration : Configurer
    {
        readonly ServerRoute serverRoute;
        readonly LocalChannelSchema schema;

        public LocalChannelConfiguration(ServerRoute serverRoute, MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            this.serverRoute = serverRoute;
            this.schema = new LocalChannelSchema
            {
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>() 
            };
        }

        protected override void Build()
        {
            Resolve<LocalChannelBuilder>().Build(this.schema);
        }

        protected override ServerRoute GetServerPath()
        {
            return this.serverRoute;
        }

        public LocalChannelConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.schema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }
    }
}