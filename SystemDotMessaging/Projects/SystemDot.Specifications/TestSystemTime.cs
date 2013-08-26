using System;

namespace SystemDot.Specifications
{
    public class TestSystemTime : ISystemTime
    {
        DateTime currentDate;

        public TimeSpan FromSecondsSpanOverride { get; set; }

        public TimeSpan LastTimeSpanRequested { get; set; }

        public TestSystemTime(DateTime currentDate)
        {
            SystemTime.SetCurrent(this);
            this.currentDate = currentDate;
        }

        public DateTime GetCurrentDate()
        {
            return this.currentDate;
        }

        public void AdvanceTime(TimeSpan toAdd)
        {
            currentDate = currentDate.Add(toAdd);
            Messenger.Send(new TestSystemTimeAdvanced());
        }

        public TimeSpan SpanFromSeconds(int seconds)
        {
            LastTimeSpanRequested = TimeSpan.FromSeconds(seconds);

            return FromSecondsSpanOverride == TimeSpan.MinValue
                ? LastTimeSpanRequested
                : FromSecondsSpanOverride;
        }
    }
}