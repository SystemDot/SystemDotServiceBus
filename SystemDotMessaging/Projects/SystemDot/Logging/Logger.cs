using System;

namespace SystemDot.Logging
{
    public class Logger
    {
        public static ILoggingMechanism LoggingMechanism { get; set; }

        public static void Info(string message)
        {
            if (LoggingMechanism == null) return;
            if (!LoggingMechanism.ShowInfo) return;
            
            LoggingMechanism.Info(message);
        }

        public static void Info(string message, params object[] args)
        {
            if (LoggingMechanism == null) return;
            if (!LoggingMechanism.ShowInfo) return;
            
            LoggingMechanism.Info(String.Format(message, args));
        }

        public static void Debug(string message, params object[] args)
        {
            if (LoggingMechanism == null) return;
            if (!LoggingMechanism.ShowDebug) return;

            LoggingMechanism.Debug(String.Format(message, args));
        }

        public static void Error(Exception exception)
        {
            if (LoggingMechanism == null) return;

            LoggingMechanism.Error(exception);
        }
    }
}