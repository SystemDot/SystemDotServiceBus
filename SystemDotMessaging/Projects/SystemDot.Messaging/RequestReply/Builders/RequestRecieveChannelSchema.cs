using System.Collections.Generic;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestRecieveChannelSchema : RecieveChannelSchema
    {
        public bool HandleRequestsOnMainThread { get; set; }
    }
}