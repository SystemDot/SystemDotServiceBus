using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public static class MessageFirstSequenceExtensions
    {
        public static int GetFirstSequence(this MessagePayload payload)
        {
            return payload.GetHeader<FirstSequenceHeader>().Sequence;
        }

        public static void SetFirstSequence(this MessagePayload payload, int sequence)
        {
            payload.AddHeader(new FirstSequenceHeader { Sequence = sequence });
        }
    }
}