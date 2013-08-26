using System;

namespace SystemDot
{
    public class SystemTime : ISystemTime
    {
        public static ISystemTime Current { get; private set; }

        public static void SetCurrent(ISystemTime toSet)
        {
            Current = toSet;
        }

        public SystemTime()
        {
            SetCurrent(this);
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