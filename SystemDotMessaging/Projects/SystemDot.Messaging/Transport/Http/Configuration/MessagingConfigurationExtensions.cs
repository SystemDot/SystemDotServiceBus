using System;
using System.Collections.Generic;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static HttpTransportConfiguration UsingHttpTransport(this MessagingConfiguration config)
        {
            HttpTransportComponents.Register(IocContainerLocator.Locate());
            return new HttpTransportConfiguration();
        }
    }
}