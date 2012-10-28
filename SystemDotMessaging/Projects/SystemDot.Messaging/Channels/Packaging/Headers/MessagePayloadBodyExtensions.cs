﻿using System.Diagnostics.Contracts;
using System.Linq;

namespace SystemDot.Messaging.Channels.Packaging.Headers
{
    public static class MessagePayloadBodyExtensions
    {
        public static byte[] GetBody(this MessagePayload payload)
        {
            Contract.Requires(payload.HasBody());

            return payload.GetHeader<BodyHeader>().Body;
        }

        public static void SetBody(this MessagePayload payload, byte[] body)
        {
            Contract.Requires(body != null);
            payload.AddHeader(new BodyHeader(body));
        }

        public static bool HasBody(this MessagePayload payload)
        {
            return payload.HasHeader<BodyHeader>();
        }

    }
}
