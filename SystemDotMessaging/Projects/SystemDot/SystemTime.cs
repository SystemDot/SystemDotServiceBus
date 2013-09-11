using System;

namespace SystemDot
{
    public class SystemTime : ISystemTime
    {
        public static ISystemTime Current { get; private set; }

        static SystemTime()
        {
            SetCurrent(new SystemTime());
        }

        public static void SetCurrent(ISystemTime toSet)
        {
            Current = toSet;
        }

        public DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public TimeSpan SpanFromSeconds(int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }
    }
}