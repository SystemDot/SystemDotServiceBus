using System;

namespace SystemDot.Logging
{
    public class Log4NetLoggingMechanism : ILoggingMechanism
    {
        public bool ShowInfo { get; set; }

        public bool ShowDebug { get; set; }

        public void Info(string message)
        {
            //TODO
        }

        public void Debug(string message)
        {
            //TODO
        }

        public void Error(Exception exception)
        {
            //TODO
        }
    }
}