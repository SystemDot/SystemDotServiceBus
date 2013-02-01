using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Sequencing
{
    public static class MessageSequenceExtensions
    {
        public static int GetSequence(this MessagePayload payload)
        {
            return payload.GetHeader<SequenceHeader>().Sequence;
        }

        public static bool HasSequence(this MessagePayload payload)
        {
            return payload.HasHeader<SequenceHeader>();
        }

        public static void SetSequence(this MessagePayload payload, int sequence)
        {
            payload.AddHeader(new SequenceHeader { Sequence = sequence });
        }
    }
}