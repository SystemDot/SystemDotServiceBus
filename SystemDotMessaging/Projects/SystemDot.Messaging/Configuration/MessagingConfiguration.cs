using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Configuration.Reading;
using SystemDot.Core;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;

namespace SystemDot.Messaging.Configuration
{
    public class MessagingConfiguration : ConfigurationBase
    {
        public List<Action> BuildActions { get; private set; }

        internal IIocResolver ExternalResolver { get; private set; }

        public MessagingConfiguration()
        {
            Components.Register();
            LoadConfigurationFromFile();

            BuildActions = new List<Action>();
            ExternalResolver = GetInternalIocContainer();
        }

        static void LoadConfigurationFromFile()
        {
            Resolve<IConfigurationReader>().Load("SystemDot.config");
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

        public HandlerBasedOnConfiguration RegisterHandlersFromContainer()
        {
            return new HandlerBasedOnConfiguration(
                this,
                ExternalResolver.GetAllRegisteredTypes().WhereNormalConcrete());
        }

        public MessageServerConfiguration UsingInProcessTransport()
        {
            InProcessTransportComponents.Register(IocContainerLocator.Locate());
            return new MessageServerConfiguration(this, MessageServer.None);
        }

        public HttpTransportConfiguration UsingHttpTransport()
        {
            return new HttpTransportConfiguration(this);
        }

        public IIocContainer GetInternalIocContainer()
        {
            return GetContainer();
        }
    }
}