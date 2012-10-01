using System.Linq;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public static class MessageSequenceExtensions
    {
        public static int GetSequence(this MessagePayload payload)
        {
            return payload.Headers.OfType<SequenceHeader>().Single().Sequence;
        }

        public static void SetSequence(this MessagePayload payload, int sequence)
        {
            payload.Headers.Add(new SequenceHeader { Sequence = sequence });
        }
    }
}