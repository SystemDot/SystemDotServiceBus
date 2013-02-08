using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static MessageServerConfiguration UsingHttpTransport(this MessagingConfiguration config, MessageServer server)
        {
            Contract.Requires(server != null);

            HttpTransportComponents.Register(IocContainerLocator.Locate());
            return new MessageServerConfiguration(server);
        }

        public static HttpTransportConfiguration UsingHttpTransport(this MessagingConfiguration config)
        {
            HttpTransportComponents.Register(IocContainerLocator.Locate());
            return new HttpTransportConfiguration(new List<Action>());
        }
    }
}