using System;

namespace SystemDot.Logging
{
    public class Logger
    {
        public static bool ShowInfo { get; set; }

        public static ILoggingMechanism LoggingMechanism { get; set; }

        public static void Info(string message)
        {
            if (!ShowInfo) return;
            if (LoggingMechanism != null)
                LoggingMechanism.Info(message);
        }

        public static void Info(string message, params object[] args)
        {
            if (!ShowInfo) return;
            if (LoggingMechanism != null)
                LoggingMechanism.Info(String.Format(message, args));
        }

        public static void Error(string message)
        {
            if (LoggingMechanism != null)
                LoggingMechanism.Error(message);
        }
    }
}