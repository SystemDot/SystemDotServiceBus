using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Sequencing
{
    public static class SequenceOriginSetOnExtensions
    {
        public static void SetSequenceOriginSetOn(this MessagePayload payload, DateTime setOn)
        {
            payload.RemoveHeader(typeof(SequenceOriginSetHeader));
            payload.AddHeader(new SequenceOriginSetHeader { SetOn = setOn });
        }

        public static DateTime GetSequenceOriginSetOn(this MessagePayload payload)
        {
            return payload.GetHeader<SequenceOriginSetHeader>().SetOn;
        }
    }
}