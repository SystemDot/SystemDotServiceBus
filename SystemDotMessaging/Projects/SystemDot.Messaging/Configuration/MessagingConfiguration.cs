using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Configuration;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;

namespace SystemDot.Messaging.Configuration
{
    public class MessagingConfiguration
    {
        public List<Action> BuildActions { get; private set; }

        internal IIocResolver ExternalResolver { get; private set; }

        public MessagingConfiguration()
        {
            BuildActions = new List<Action>();
            ExternalResolver = GetInternalIocContainer();
        }

        public MessagingConfiguration LoggingWith(ILoggingMechanism toLogWith)
        {
            Contract.Requires(toLogWith != null);

            Logger.LoggingMechanism = toLogWith;
            return this;
        }

        public MessagingConfiguration ResolveReferencesWith(IIocResolver container)
        {
            Contract.Requires(container != null);

            ExternalResolver = container;
            return this;
        }

        public HandlerBasedOnConfiguration RegisterHandlersFromAssemblyOf<TAssemblyOf>()
        {
            return new HandlerBasedOnConfiguration(
                this,
                typeof(TAssemblyOf).GetTypesInAssembly().WhereNonAbstract().WhereNonGeneric().WhereConcrete());
        }

        public MessageServerConfiguration UsingInProcessTransport()
        {
            InProcessTransportComponents.Register(IocContainerLocator.Locate());
            return new MessageServerConfiguration(this, MessageServer.None);
        }

        public HttpTransportConfiguration UsingHttpTransport()
        {
            HttpTransportComponents.Register(IocContainerLocator.Locate());
            return new HttpTransportConfiguration(this);
        }

        public IIocContainer GetInternalIocContainer()
        {
            return IocContainerLocator.Locate();
        }
    }
}