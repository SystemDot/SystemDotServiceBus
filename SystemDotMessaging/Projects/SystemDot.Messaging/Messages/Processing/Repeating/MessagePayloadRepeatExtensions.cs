using System;
using System.Linq;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Processing.Repeating
{
    public static class MessagePayloadRepeatExtensions
    {
        public static void SetLastTimeSent(this MessagePayload payload, DateTime toSet)
        {
            AddHeaderIfNonExistant(payload);
            payload.Headers.OfType<LastSentHeader>().Single().Time = toSet;
        }

        public static DateTime GetLastTimeSent(this MessagePayload payload)
        {
            return payload.Headers.OfType<LastSentHeader>().Single().Time;
        }

        public static void IncreaseAmountSent(this MessagePayload payload)
        {
            AddHeaderIfNonExistant(payload);
            payload.Headers.OfType<LastSentHeader>().Single().Amount++;
        }

        public static int GetAmountSent(this MessagePayload payload)
        {
            return payload.Headers.OfType<LastSentHeader>().Single().Amount;
        }

        static void AddHeaderIfNonExistant(MessagePayload payload)
        {
            if(payload.Headers.OfType<LastSentHeader>().Any()) return;
            payload.AddHeader(new LastSentHeader());
        }
    }
}