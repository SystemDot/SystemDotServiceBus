using System;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Repeating
{
    public static class MessagePayloadRepeatExtensions
    {
        public static void SetLastTimeSent(this MessagePayload payload, DateTime toSet)
        {
            AddHeaderIfNonExistant(payload);
            payload.GetHeader<LastSentHeader>().Time = toSet;
        }

        public static DateTime GetLastTimeSent(this MessagePayload payload)
        {
            return payload.GetHeader<LastSentHeader>().Time;
        }

        public static void IncreaseAmountSent(this MessagePayload payload)
        {
            AddHeaderIfNonExistant(payload);
            payload.GetHeader<LastSentHeader>().Amount++;
        }

        public static int GetAmountSent(this MessagePayload payload)
        {
            return payload.GetHeader<LastSentHeader>().Amount;
        }

        static void AddHeaderIfNonExistant(MessagePayload payload)
        {
            if(payload.HasHeader<LastSentHeader>()) return;
            payload.AddHeader(new LastSentHeader());
        }
    }
}