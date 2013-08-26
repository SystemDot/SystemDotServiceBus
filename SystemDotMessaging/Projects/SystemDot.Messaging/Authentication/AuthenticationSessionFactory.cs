using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

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

        public AuthenticationSession Create(MessageServer server, TimeSpan expiresAfter)
        {
            Contract.Requires(server != null);
            Contract.Requires(expiresAfter != null);

            return new AuthenticationSession(server, GetExpiresOn(expiresAfter));
        }

        DateTime GetExpiresOn(TimeSpan expiresAfter)
        {
            return expiresAfter == TimeSpan.MaxValue 
                ? DateTime.MaxValue
                : systemTime.GetCurrentDate().Add(expiresAfter);
        }
    }
}