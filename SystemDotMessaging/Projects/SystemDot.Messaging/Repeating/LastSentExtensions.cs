using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Repeating
{
    public static class LastSentExtensions
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
            AddHeaderIfNonExistant(payload);
            return payload.GetHeader<LastSentHeader>().Amount;
        }

        static void AddHeaderIfNonExistant(MessagePayload payload)
        {
            if(payload.HasHeader<LastSentHeader>()) return;
            payload.AddHeader(new LastSentHeader());
        }
    }
}