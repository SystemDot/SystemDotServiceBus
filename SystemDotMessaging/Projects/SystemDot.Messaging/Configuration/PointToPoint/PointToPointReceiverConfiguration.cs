using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExceptionHandling;
using SystemDot.Messaging.Configuration.Filtering;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointReceiverConfiguration : Configurer, 
        IExceptionHandlingConfigurer,
        IFilterMessagesConfigurer
    {
        readonly MessageServer server;
        readonly PointToPointReceiverChannelSchema schema;

        public PointToPointReceiverConfiguration(
            EndpointAddress address, 
            MessageServer server, 
            MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            Contract.Requires(address != null);
            Contract.Requires(server != null);
            Contract.Requires(messagingConfiguration != null);

            this.server = server;

            schema = new PointToPointReceiverChannelSchema
            {
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>,
                Address = address,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };
        }

        protected internal override void Build()
        {
            Resolve<PointToPointReceiveChannelBuilder>().Build(schema);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }

        public PointToPointReceiverConfiguration Sequenced()
        {
            schema.IsSequenced = true;
            return this;
        }

        public PointToPointReceiverConfiguration WithDurability()
        {
            schema.IsDurable = true;
            schema.IsSequenced = true;
            return this;
        }

        public PointToPointReceiverConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            schema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }

        public FilterMessagesConfiguration<PointToPointReceiverConfiguration> OnlyForMessages()
        {
            return new FilterMessagesConfiguration<PointToPointReceiverConfiguration>(this);
        }

        public void SetMessageFilterStrategy(IMessageFilterStrategy strategy)
        {
            schema.FilterStrategy = strategy;
        }

        public OnExceptionConfiguration<PointToPointReceiverConfiguration> OnException()
        {
            return new OnExceptionConfiguration<PointToPointReceiverConfiguration>(this);
        }

        public void SetContinueOnException()
        {
            schema.ContinueOnException = true;
        }

        public PointToPointReceiverConfiguration BlockMessagesIf(bool shouldBlock)
        {
            schema.BlockMessages = true;
            return this;
        }
    }
}