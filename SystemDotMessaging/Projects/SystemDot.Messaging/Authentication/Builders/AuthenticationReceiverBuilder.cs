using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.Authentication;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Authentication.Builders
{
    class AuthenticationReceiverBuilder
    {
        readonly AuthenticatedServerRegistry serverRegistry;
        readonly AuthenticationSessionCache cache;
        readonly ISerialiser serialiser;

        public AuthenticationReceiverBuilder(AuthenticationSessionCache cache, ISerialiser serialiser, AuthenticatedServerRegistry serverRegistry)
        {
            Contract.Requires(cache != null);
            Contract.Requires(serialiser != null);

            this.cache = cache;
            this.serialiser = serialiser;
            this.serverRegistry = serverRegistry;
        }

        public void Build<TAuthenticationRequest, TAuthenticationResponse>(Configurer configurer, MessageServer server, ExpiryPlan expiryPlan)
        {
            Contract.Requires(configurer != null);
            Contract.Requires(server != null);
            Contract.Requires(expiryPlan != null);

            BuildChannel<TAuthenticationRequest, TAuthenticationResponse>(configurer, expiryPlan);
            RegisterAuthenticatedServer(server);
            CreateNewSessionForServer(server);
        }

        void BuildChannel<TAuthenticationRequest, TAuthenticationResponse>(Configurer configurer, ExpiryPlan expiryPlan)
        {
            configurer.OpenDirectChannel(ChannelNames.AuthenticationChannelName)
                .ForRequestReplyReceiving()
                .OnlyForMessages(FilteredBy.Type<TAuthenticationRequest>())
                .WithReplyHook(new AuthenticationResponseHook<TAuthenticationResponse>(serialiser, cache, expiryPlan));
        }

        void RegisterAuthenticatedServer(MessageServer server)
        {
            serverRegistry.Register(server);
        }

        void CreateNewSessionForServer(MessageServer server)
        {
            cache.CacheNewSessionFor(server, ExpiryPlan.Never());
        }
    }
}