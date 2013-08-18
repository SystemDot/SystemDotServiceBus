using System;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Configuration.Authentication;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Specifications.authentication
{
    public static class MessagePayloadExtensions
    {
        public static MessagePayload MakeAuthenticationRequest<TRequest>(this MessagePayload payload)
            where TRequest : new()
        {
            payload.SetMessageBody(new TRequest())
                .SetFromChannel(ChannelNames.AuthenticationChannelName)
                .SetToChannel(ChannelNames.AuthenticationChannelName);

            payload.SetIsDirectChannelMessage();

            return payload;
        }

        public static MessagePayload MakeAuthenticationResponse<TResponse>(this MessagePayload payload)
            where TResponse : new()
        {
            payload.MakeAuthenticationRequest<TResponse>();
            payload.SetAuthenticationSession(Guid.NewGuid());

            return payload;
        }
    }
}