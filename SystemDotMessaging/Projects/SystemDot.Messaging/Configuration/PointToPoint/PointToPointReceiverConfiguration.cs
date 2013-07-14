using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointReceiverConfiguration : Configurer
    {
        readonly ServerRoute serverRoute;
        readonly PointToPointReceiverChannelSchema schema;

        public PointToPointReceiverConfiguration(
            EndpointAddress address, 
            ServerRoute serverRoute, 
            MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            Contract.Requires(address != null);
            Contract.Requires(serverRoute != null);
            Contract.Requires(messagingConfiguration != null);

            this.serverRoute = serverRoute;

            this.schema = new PointToPointReceiverChannelSchema
            {
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>,
                Address = address,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };
        }

        protected override void Build()
        {
            Resolve<PointToPointReceiveChannelBuilder>().Build(this.schema);
        }

        protected override ServerRoute GetServerPath()
        {
            return this.serverRoute;
        }

        public PointToPointReceiverConfiguration WithDurability()
        {
            this.schema.IsDurable = true;
            return this;
        }

        public PointToPointReceiverConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            schema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }

        public PointToPointReceiverConfiguration OnlyForMessages(IMessageFilterStrategy toFilterWith)
        {
            Contract.Requires(toFilterWith != null);

            schema.FilterStrategy = toFilterWith;
            return this;
        }
    }
}