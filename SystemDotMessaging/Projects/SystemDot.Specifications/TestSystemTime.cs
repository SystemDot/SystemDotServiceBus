using System;

namespace SystemDot.Specifications
{
    public class TestSystemTime : ISystemTime
    {
        DateTime currentDate;
        readonly TimeSpan fromSecondsSpanOverride;

        public TimeSpan LastTimeSpanRequested { get; set; }
        
        public TestSystemTime(DateTime currentDate)
            : this(currentDate, TimeSpan.MinValue)
        {
        }

        public TestSystemTime(DateTime currentDate, TimeSpan fromSecondsSpanOverride)
        {
            this.currentDate = currentDate;
            this.fromSecondsSpanOverride = fromSecondsSpanOverride;
        }

        public DateTime GetCurrentDate()
        {
            return this.currentDate;
        }

        public void AddToCurrentDate(TimeSpan toAdd)
        {
            this.currentDate = this.currentDate.Add(toAdd);
        }

        public TimeSpan SpanFromSeconds(int seconds)
        {
            LastTimeSpanRequested = TimeSpan.FromSeconds(seconds);

            return this.fromSecondsSpanOverride == TimeSpan.MinValue
                ? LastTimeSpanRequested
                : this.fromSecondsSpanOverride;
        }
    }
}