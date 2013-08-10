using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement.Builders;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Direct;
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
            Resolve<ITransportBuilder>().Build(GetMessageServer());
            
            Resolve<SubscriptionRequestReceiveChannelBuilder>().Build();
            Resolve<AcknowledgementSendChannelBuilder>().Build();
            Resolve<AcknowledgementRecieveChannelBuilder>().Build();

            this.messagingConfiguration.BuildActions.ForEach(a => a());
            
            Messenger.Send(new MessagingInitialising());

            Resolve<ITaskRepeater>().Start();

            Messenger.Send(new MessagingInitialised());
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(new EndpointAddress(name, GetMessageServer()), GetMessageServer(), messagingConfiguration);
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

        public LocalChannelConfiguration OpenLocalChannel()
        {
            return new LocalChannelConfiguration(GetMessageServer(), messagingConfiguration);
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