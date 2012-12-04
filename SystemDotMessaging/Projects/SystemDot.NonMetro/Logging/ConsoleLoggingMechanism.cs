using System;

namespace SystemDot.Logging
{
    public class ConsoleLoggingMechanism : ILoggingMechanism
    {
        public bool ShowInfo { get; set; }

        public bool ShowDebug { get; set; }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }
        
        public void Error(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}