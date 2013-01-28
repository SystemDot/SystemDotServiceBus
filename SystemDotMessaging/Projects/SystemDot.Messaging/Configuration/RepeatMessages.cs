using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Configuration
{
    public class RepeatMessages
    {
        public static IRepeatStrategy Every(TimeSpan time)
        {
            Contract.Requires(time != null);

            return new ConstantTimeRepeatStrategy(time);
        }
    }
}