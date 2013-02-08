using System;

namespace SystemDot
{
    public class SystemTime : ISystemTime
    {
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