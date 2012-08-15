using System;
using System.Collections.Generic;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IRequestRecieveChannelBuilder
    {
        Guid Build();
    }
}