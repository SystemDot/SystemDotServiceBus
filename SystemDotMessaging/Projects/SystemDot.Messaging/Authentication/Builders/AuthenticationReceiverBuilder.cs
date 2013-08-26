using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Authentication.Expiry;
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
            Contract.Requires(serverRegistry != null);

            this.cache = cache;
            this.serialiser = serialiser;
            this.serverRegistry = serverRegistry;
        }

        public void Build<TAuthenticationRequest, TAuthenticationResponse>(Configurer configurer, MessageServer server, AuthenticationReceiverSchema schema)
        {
            Contract.Requires(configurer != null);
            Contract.Requires(server != null);
            Contract.Requires(schema != null);

            BuildChannel<TAuthenticationRequest, TAuthenticationResponse>(configurer, schema);
            RunActionOnExpiry(schema.ToRunOnExpiry);
            RegisterAuthenticatedServer(server);
            CreateNewSessionForServer(server);
        }

        void BuildChannel<TAuthenticationRequest, TAuthenticationResponse>(Configurer configurer, AuthenticationReceiverSchema schema)
        {
            configurer.OpenDirectChannel(ChannelNames.AuthenticationChannelName)
                .ForRequestReplyReceiving()
                .OnlyForMessages(FilteredBy.Type<TAuthenticationRequest>())
                .WithReplyHook(new AuthenticationResponseHook<TAuthenticationResponse>(serialiser, cache, schema.ExpiresAfter));
        }

        void RunActionOnExpiry(Action<AuthenticationSession> toRunOnExpiry)
        {
            Messenger.Register<AuthenticationSessionExpired>(e => toRunOnExpiry(e.Session));
        }

        void RegisterAuthenticatedServer(MessageServer server)
        {
            serverRegistry.Register(server);
        }

        void CreateNewSessionForServer(MessageServer server)
        {
            if (cache.HasCurrentSessionFor(server)) return;
            cache.CacheNewSessionFor(server, TimeSpan.MaxValue);
        }
    }
}