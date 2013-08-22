using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    public static class MessagePayloadExtensions
    {
        public static AuthenticationSession GetAuthenticationSession(this MessagePayload payload)
        {
            return payload.GetHeader<AuthenticationSessionHeader>().Session;
        }

        public static bool HasAuthenticationSession(this MessagePayload payload)
        {
            return payload.HasHeader<AuthenticationSessionHeader>();
        }

        public static void SetAuthenticationSession(this MessagePayload payload, AuthenticationSession toSet)
        {
            Contract.Requires(toSet != null);
            payload.AddHeader(new AuthenticationSessionHeader(toSet));
        }
    }
}