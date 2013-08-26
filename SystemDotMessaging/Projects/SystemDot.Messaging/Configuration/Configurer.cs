using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement.Builders;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Configuration.Authentication;
using SystemDot.Messaging.Configuration.Direct;
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

        protected internal abstract void Build();

        public void Initialise()
        {
            Resolve<ITransportBuilder>().Build(GetMessageServer());

            Resolve<AuthenticationSessionCache>().Initialise();
            Resolve<SubscriptionRequestReceiveChannelBuilder>().Build();
            Resolve<AcknowledgementSendChannelBuilder>().Build();
            Resolve<AcknowledgementRecieveChannelBuilder>().Build();
            
            messagingConfiguration.BuildActions.ForEach(a => a());
            
            Messenger.Send(new MessagingInitialising());

            Resolve<ITaskRepeater>().Start();

            Messenger.Send(new MessagingInitialised());
        }

        public AuthenticateToServerConfiguration AuthenticateToServer(string serverRequiringAuthentication)
        {
            Contract.Requires(!string.IsNullOrEmpty(serverRequiringAuthentication));

            return new AuthenticateToServerConfiguration(messagingConfiguration, GetMessageServer(), serverRequiringAuthentication);
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(new EndpointAddress(name, GetMessageServer()), GetMessageServer(), messagingConfiguration);
        }

        public LocalDirectChannelConfiguration OpenDirectChannel()
        {
            return new LocalDirectChannelConfiguration(GetMessageServer(), messagingConfiguration);
        }

        public DirectChannelConfiguration OpenDirectChannel(string name)
        {
            return new DirectChannelConfiguration(new EndpointAddress(name, GetMessageServer()), messagingConfiguration);
        }

        public Configurer RegisterHandlers(Action<MessageHandlerRouter> registrationAction)
        {
            registrationAction(Resolve<MessageHandlerRouter>());
            return this;
        }

        protected abstract MessageServer GetMessageServer();

        internal UnitOfWorkRunner CreateUnitOfWorkRunner<TUnitOfWorkFactory>() 
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            return new UnitOfWorkRunner(GetUnitOfWorkFactory<TUnitOfWorkFactory>());
        }

        IUnitOfWorkFactory GetUnitOfWorkFactory<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            return messagingConfiguration.ExternalResolver.ComponentExists<TUnitOfWorkFactory>()
                ? messagingConfiguration.ExternalResolver.Resolve<TUnitOfWorkFactory>().As<IUnitOfWorkFactory>()
                : new NullUnitOfWorkFactory();
        }
    }
}