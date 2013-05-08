using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Acknowledgement.Builders;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.UnitOfWork;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration
{
    public abstract class Configurer : ConfigurationBase
    {
        readonly List<Action> buildActions;

        protected Configurer(List<Action> buildActions)
        {
            Contract.Requires(buildActions != null);

            this.buildActions = buildActions;
            this.buildActions.Add(Build);
        }

        protected abstract void Build();

        public void Initialise()
        {
            Resolve<SubscriptionHandlerChannelBuilder>().Build();
            Resolve<AcknowledgementSendChannelBuilder>().Build();
            Resolve<AcknowledgementRecieveChannelBuilder>().Build();

            this.buildActions.ForEach(a => a());

            Resolve<ITransportBuilder>().Build(GetServerPath());
            
            Messenger.Send(new MessagingInitialising());

            Resolve<ITaskRepeater>().Start();

            Messenger.Send(new MessagingInitialised());
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(
                new EndpointAddress(name, GetServerPath()),
                GetServerPath(),
                this.buildActions);
        }

        public Configurer RegisterHandlers(Action<MessageHandlerRouter> registrationAction)
        {
            registrationAction(Resolve<MessageHandlerRouter>());
            return this;
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            return new LocalChannelConfiguration(GetServerPath(), this.buildActions);
        }

        protected abstract ServerPath GetServerPath();

        internal static UnitOfWorkRunner<TUnitOfWorkFactory> CreateUnitOfWorkRunner<TUnitOfWorkFactory>() 
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            return new UnitOfWorkRunner<TUnitOfWorkFactory>(Resolve<IIocContainer>());
        }
    }
}