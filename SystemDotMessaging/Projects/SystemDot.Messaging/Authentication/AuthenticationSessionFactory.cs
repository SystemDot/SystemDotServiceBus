using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionFactory
    {
        readonly ISystemTime systemTime;

        public AuthenticationSessionFactory(ISystemTime systemTime)
        {
            Contract.Requires(systemTime != null);
            this.systemTime = systemTime;
        }

        public AuthenticationSession Create(TimeSpan expiresAfter)
        {
            Contract.Requires(expiresAfter != null);

            return new AuthenticationSession(expiresAfter);
        }
    }
}