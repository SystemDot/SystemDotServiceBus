using SystemDot.Http.Builders;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    static class HttpTransportComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IHttpServerBuilder, HttpServerBuilder>();
        }
    }
}