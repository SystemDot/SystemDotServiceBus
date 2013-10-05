using System;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Configuration.Authentication;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Specifications.authentication
{
    public static class MessagePayloadExtensions
    {
        public static MessagePayload SetAuthenticationRequestChannels(this MessagePayload payload)
        {
            payload
                .SetFromChannel(ChannelNames.AuthenticationChannelName)
                .SetToChannel(ChannelNames.AuthenticationChannelName);

            payload.SetIsDirectChannelMessage();

            return payload;
        }

        public static MessagePayload SetAuthenticationSession(this MessagePayload payload)
        {
            payload.SetAuthenticationSession(new AuthenticationSession(DateTime.MaxValue));

            return payload;
        }
    }
}