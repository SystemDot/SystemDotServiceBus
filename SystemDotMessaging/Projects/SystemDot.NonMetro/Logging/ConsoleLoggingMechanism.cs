using System;

namespace SystemDot.Logging
{
    public class ConsoleLoggingMechanism : ILoggingMechanism
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }
    }
}