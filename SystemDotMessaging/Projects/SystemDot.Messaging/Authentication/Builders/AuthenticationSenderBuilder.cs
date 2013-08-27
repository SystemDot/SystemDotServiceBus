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
            RunActionOnExpiry(schema.ToRunOnExpiry);
        }

        void BuildChannel<TAuthenticationRequest>(Configurer configurer, AuthenticationSenderSchema schema)
        {
            configurer.OpenDirectChannel(ChannelNames.AuthenticationChannelName)
                .ForRequestReplySendingTo(GetAuthenticationReceiverChannel(schema.Server))
                .WithReceiveHook(new AuthenticationReceiveHook(cache))
                .OnlyForMessages(FilteredBy.Type<TAuthenticationRequest>())
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

        void RunActionOnExpiry(Action<AuthenticationSession> toRunOnExpiry)
        {
            Messenger.Register<AuthenticationSessionExpired>(e => toRunOnExpiry(e.Session));
        }
    }
}