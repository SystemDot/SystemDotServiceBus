using System;

namespace SystemDot.Logging
{
    public class Logger
    {
        public static bool Enabled { get; set; }

        public static void Info(string message)
        {
            if (!Enabled) return;
            Console.WriteLine(message);
        }

        public static void Info(string message, params object[] args)
        {
            if (!Enabled) return;
            Console.WriteLine(message, args);
        }
    }
}