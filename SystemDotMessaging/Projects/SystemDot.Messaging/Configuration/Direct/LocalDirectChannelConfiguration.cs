using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.Direct
{
    public class LocalDirectChannelConfiguration : Configurer
    {
        readonly MessageServer server;
        readonly LocalDirectChannelSchema schema;

        public LocalDirectChannelConfiguration(MessageServer server, MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            this.server = server;
            schema = new LocalDirectChannelSchema
            {
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>() 
            };
        }

        protected internal override void Build()
        {
            Resolve<LocalDirectChannelBuilder>().Build(schema);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }

        public LocalDirectChannelConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            schema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }
    }
}