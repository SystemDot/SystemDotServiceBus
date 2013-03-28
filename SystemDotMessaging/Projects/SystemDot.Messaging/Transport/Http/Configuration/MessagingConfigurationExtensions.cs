using System;
using System.Collections.Generic;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static HttpTransportConfiguration UsingHttpTransport(this MessagingConfiguration config)
        {
            HttpTransportComponents.Register(IocContainerLocator.Locate());
            return new HttpTransportConfiguration(config.BuildActions);
        }
    }
}