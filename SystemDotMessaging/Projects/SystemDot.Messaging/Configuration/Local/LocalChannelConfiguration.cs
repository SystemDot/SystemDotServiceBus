using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Local.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.Local
{
    public class LocalChannelConfiguration : Configurer
    {
        readonly ServerPath serverPath;
        readonly LocalChannelSchema schema;

        public LocalChannelConfiguration(ServerPath serverPath, MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            this.serverPath = serverPath;
            this.schema = new LocalChannelSchema
            {
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>() 
            };
        }

        protected override void Build()
        {
            Resolve<LocalChannelBuilder>().Build(this.schema);
        }

        protected override ServerPath GetServerPath()
        {
            return this.serverPath;
        }

        public LocalChannelConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.schema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }
    }
}