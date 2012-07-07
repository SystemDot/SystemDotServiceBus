using System;

namespace SystemDot.Logging
{
    public class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Info(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
    }
}