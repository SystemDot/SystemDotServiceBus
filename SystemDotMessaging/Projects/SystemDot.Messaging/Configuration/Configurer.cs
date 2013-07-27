using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Configuration;
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
        readonly MessagingConfiguration messagingConfiguration;

        protected Configurer(MessagingConfiguration messagingConfiguration)
        {
            Contract.Requires(messagingConfiguration != null);

            this.messagingConfiguration = messagingConfiguration;
            this.messagingConfiguration.BuildActions.Add(Build);
        }

        protected abstract void Build();

        public void Initialise()
        {
            Resolve<SubscriptionRequestReceiveChannelBuilder>().Build();
            Resolve<AcknowledgementSendChannelBuilder>().Build();
            Resolve<AcknowledgementRecieveChannelBuilder>().Build();

            this.messagingConfiguration.BuildActions.ForEach(a => a());

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
                this.messagingConfiguration);
        }

        public Configurer RegisterHandlers(Action<MessageHandlerRouter> registrationAction)
        {
            registrationAction(Resolve<MessageHandlerRouter>());
            return this;
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            return new LocalChannelConfiguration(GetServerPath(), this.messagingConfiguration);
        }

        protected abstract ServerRoute GetServerPath();

        internal UnitOfWorkRunner CreateUnitOfWorkRunner<TUnitOfWorkFactory>() 
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            return new UnitOfWorkRunner(GetUnitOfWorkFactory<TUnitOfWorkFactory>());
        }

        IUnitOfWorkFactory GetUnitOfWorkFactory<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            return this.messagingConfiguration.ExternalResolver.ComponentExists<TUnitOfWorkFactory>()
                ? this.messagingConfiguration.ExternalResolver.Resolve<TUnitOfWorkFactory>().As<IUnitOfWorkFactory>()
                : new NullUnitOfWorkFactory();
        }
    }
}