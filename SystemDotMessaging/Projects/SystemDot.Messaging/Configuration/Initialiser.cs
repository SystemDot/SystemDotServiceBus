using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Local.Builders;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration
{
    public abstract class Initialiser : Configurer
    {
        readonly List<Action> buildActions;

        protected Initialiser(List<Action> buildActions)
        {
            Contract.Requires(buildActions != null);

            this.buildActions = buildActions;
            this.buildActions.Add(Build);
        }

        protected abstract void Build();

        public IBus Initialise()
        {
            this.buildActions.ForEach(a => a());
            
            Resolve<ITaskRepeater>().Start();
            Resolve<AcknowledgementSendChannelBuilder>().Build();
            Resolve<AcknowledgementRecieveChannelBuilder>().Build();
            Resolve<LocalChannelBuilder>().Build();

            return Resolve<IBus>();
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(
                BuildEndpointAddress(name, GetAddress().ServerName),
                GetAddress().ServerName,
                this.buildActions);
        }

        protected abstract EndpointAddress GetAddress();

        protected static UnitOfWorkRunner<TUnitOfWorkFactory> CreateUnitOfWorkRunner<TUnitOfWorkFactory>() 
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            return new UnitOfWorkRunner<TUnitOfWorkFactory>(Resolve<IIocContainer>());
        }
    }
}