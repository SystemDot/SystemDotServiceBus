using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Local.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.Local
{
    public class LocalChannelConfiguration : Configurer
    {
        readonly MessageServer server;
        readonly LocalChannelSchema schema;

        public LocalChannelConfiguration(MessageServer server, MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            this.server = server;
            schema = new LocalChannelSchema
            {
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>() 
            };
        }

        protected override void Build()
        {
            Resolve<LocalChannelBuilder>().Build(schema);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }

        public LocalChannelConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            schema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }
    }
}