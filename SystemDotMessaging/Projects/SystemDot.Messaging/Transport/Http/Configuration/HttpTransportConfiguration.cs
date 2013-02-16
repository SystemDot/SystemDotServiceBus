using System;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Clients.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public class HttpTransportConfiguration : Configurer
    {
        public HttpTransportConfiguration AsARemoteServer(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance));
            
            HttpRemoteServerComponents.Configure(IocContainerLocator.Locate());
            Resolve<HttpRemoteServerBuilder>().Build(instance);
            return this;
        }

        public RemoteClientConfiguration AsARemoteClient(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance)); 
            
            HttpRemoteClientComponents.Configure(IocContainerLocator.Locate());
            return new RemoteClientConfiguration(MessageServer.Local(instance));
        }

        public MessageServerConfiguration AsAServer(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance));

            HttpServerComponents.Configure(IocContainerLocator.Locate());

            return new MessageServerConfiguration(
                new ServerPath(
                    MessageServer.Local(instance), 
                    MessageServer.Local(instance)));
        }
    }
}