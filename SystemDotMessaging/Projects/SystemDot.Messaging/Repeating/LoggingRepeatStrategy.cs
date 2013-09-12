using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Sequencing;

namespace SystemDot.Messaging.Repeating
{
    public class LoggingRepeatStrategy
    {
        protected static void LogMessage(MessagePayload toInput)
        {
            Logger.Debug(
                "Repeating message: {0} on {1} with sequence {2}",
                toInput.Id,
                toInput.HasHeader<FromAddressHeader>() ? toInput.GetFromAddress().Channel : "n/a",
                toInput.HasSequence() ? toInput.GetSequence().ToString() : "n/a");
        }
    }
}