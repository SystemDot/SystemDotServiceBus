using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Authentication.Caching;
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

        public void Build<TAuthenticationRequest>(Configurer configurer, string serverRequiringAuthentication)
        {
            Contract.Requires(configurer != null);
            Contract.Requires(!String.IsNullOrEmpty(serverRequiringAuthentication));

            BuildChannel<TAuthenticationRequest>(configurer, serverRequiringAuthentication);
            RegisterAuthenticatedServer(serverRequiringAuthentication);
        }

        void BuildChannel<TAuthenticationRequest>(Configurer configurer, string serverRequiringAuthentication)
        {
            configurer.OpenDirectChannel(ChannelNames.AuthenticationChannelName)
                .ForRequestReplySendingTo(GetAuthenticationReceiverChannel(serverRequiringAuthentication))
                .WithReceiveHook(new AuthenticationReceiveHook(cache))
                .OnlyForMessages(FilteredBy.Type<TAuthenticationRequest>());
        }

        string GetAuthenticationReceiverChannel(string serverRequiringAuthentication)
        {
            return String.Concat(ChannelNames.AuthenticationChannelName, "@", serverRequiringAuthentication);
        }

        void RegisterAuthenticatedServer(string serverRequiringAuthentication)
        {
            serverRegistry.Register(serverRequiringAuthentication);
        }
    }
}