using System;

namespace SystemDot.Logging
{
    public interface ILoggingMechanism
    {
        bool ShowInfo { get; set; }
        bool ShowDebug { get; set; }

        void Info(string message);

        void Debug(string message);

        void Error(Exception exception);
    }
}