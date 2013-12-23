using System.Diagnostics.Contracts;
using SystemDot.Messaging.Authentication.Caching;

namespace SystemDot.Messaging.Authentication.RequestReply
{
    class ReplyAuthenticationSessionAttacherFactory
    {
        readonly AuthenticationSessionCache cache;
        readonly AuthenticatedServerRegistry registry;

        public ReplyAuthenticationSessionAttacherFactory(
            AuthenticationSessionCache cache,
            AuthenticatedServerRegistry registry)
        {
            this.cache = cache;
            this.registry = registry;
            Contract.Requires(cache != null);
            Contract.Requires(registry != null);
        }

        public ReplyAuthenticationSessionAttacher Create()
        {
            return new ReplyAuthenticationSessionAttacher(cache, registry);
        }
    }
}