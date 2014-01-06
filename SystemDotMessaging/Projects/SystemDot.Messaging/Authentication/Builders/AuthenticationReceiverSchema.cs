using System;

namespace SystemDot.Messaging.Authentication.Builders
{
    class AuthenticationReceiverSchema
    {
        public TimeSpan ExpiresAfter { get; set; }
        public Action<AuthenticationSession> ToRunOnExpiry { get; set; }
        public bool BlockMessages { get; set; }
    }
}