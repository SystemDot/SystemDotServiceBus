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
            Logger.Info(
                "Repeating message on {0} with sequence {1}",
                toInput.HasHeader<AddressHeader>() ? toInput.GetFromAddress().Channel : "n/a",
                toInput.HasSequence() ? toInput.GetSequence().ToString() : "n/a");
        }
    }
}