using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    public static class MessagePayloadExtensions
    {
        public static Guid GetAuthenticationSession(this MessagePayload payload)
        {
            return payload.GetHeader<AuthenticationSessionHeader>().Session;
        }

        public static bool HasAuthenticationSession(this MessagePayload payload)
        {
            return payload.HasHeader<AuthenticationSessionHeader>();
        }

        public static void SetAuthenticationSession(this MessagePayload payload, Guid toSet)
        {
            Contract.Requires(toSet != Guid.Empty);
            payload.AddHeader(new AuthenticationSessionHeader(toSet));
        }

        public static void SetIsInvalidAuthenticationSessionNotification(this MessagePayload payload)
        {
            payload.AddHeader(new InvalidAuthenticationSessionNotificationHeader());
        }

        public static bool IsInvalidAuthenticationSessionNotification(this MessagePayload payload)
        {
            return payload.HasHeader<InvalidAuthenticationSessionNotificationHeader>();
        }
    }

    public class InvalidAuthenticationSessionNotificationHeader : IMessageHeader
    {
    }
}