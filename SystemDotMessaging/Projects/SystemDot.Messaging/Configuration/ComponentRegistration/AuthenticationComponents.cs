using SystemDot.Ioc;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Builders;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Authentication.Expiry;
using SystemDot.Messaging.Authentication.RequestReply;
using SystemDot.Messaging.RequestReply.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class AuthenticationComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<AuthenticationSessionCache, AuthenticationSessionCache>();
            container.RegisterInstance<AuthenticationSenderBuilder, AuthenticationSenderBuilder>();
            container.RegisterInstance<AuthenticationReceiverBuilder, AuthenticationReceiverBuilder>();
            container.RegisterInstance<AuthenticatedServerRegistry, AuthenticatedServerRegistry>();
            container.RegisterInstance<AuthenticationSessionExpirer, AuthenticationSessionExpirer>();
            container.RegisterInstance<AuthenticationSessionFactory, AuthenticationSessionFactory>();
        }
    }
}