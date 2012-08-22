using System;

namespace SystemDot.Logging
{
    public class ConsoleLoggingMechanism : ILoggingMechanism
    {
        public bool ShowInfo { get; set; }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }
    }
}