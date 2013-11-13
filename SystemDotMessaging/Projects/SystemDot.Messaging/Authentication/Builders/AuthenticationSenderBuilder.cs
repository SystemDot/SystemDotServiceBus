using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Authentication.Expiry;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.Authentication;

namespace SystemDot.Messaging.Authentication.Builders
{
    class AuthenticationSenderBuilder
    {
        readonly AuthenticationSessionCache cache;
        readonly AuthenticatedServerRegistry serverRegistry;

        public AuthenticationSenderBuilder(AuthenticationSessionCache cache, AuthenticatedServerRegistry serverRegistry)
        {
            Contract.Requires(cache != null);
            Contract.Requires(serverRegistry != null);

            this.cache = cache;
            this.serverRegistry = serverRegistry;
        }

        public void Build<TAuthenticationRequest>(Configurer configurer, AuthenticationSenderSchema schema)
        {
            Contract.Requires(configurer != null);
            Contract.Requires(schema != null);

            BuildChannel<TAuthenticationRequest>(configurer, schema);
            RegisterAuthenticatedServer(schema);
            RunActionOnExpiry(schema);
        }

        void BuildChannel<TAuthenticationRequest>(Configurer configurer, AuthenticationSenderSchema schema)
        {
            configurer.OpenDirectChannel(ChannelNames.AuthenticationChannelName)
                .ForRequestReplySendingTo(GetAuthenticationReceiverChannel(schema.Server))
                .WithReceiveHook(new AuthenticationReceiveHook(cache))
                .OnlyForMessages().OfType<TAuthenticationRequest>()
                .Build();
        }

        string GetAuthenticationReceiverChannel(string serverRequiringAuthentication)
        {
            return String.Concat(ChannelNames.AuthenticationChannelName, "@", serverRequiringAuthentication);
        }

        void RegisterAuthenticatedServer(AuthenticationSenderSchema schema)
        {
            serverRegistry.Register(schema.Server);
        }

        void RunActionOnExpiry(AuthenticationSenderSchema schema)
        {
            Messenger.Register<AuthenticationSessionExpired>(e =>
            {
                if (schema.Server == e.Server.Name) schema.ToRunOnExpiry(e.Session);
            });
        }
    }
}