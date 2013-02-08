using System;

namespace SystemDot
{
    public interface ISystemTime
    {
        DateTime GetCurrentDate();

        TimeSpan SpanFromSeconds(int seconds);
    }
}