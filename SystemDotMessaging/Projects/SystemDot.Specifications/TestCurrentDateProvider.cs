using System;

namespace SystemDot.Specifications
{
    public class TestCurrentDateProvider : ICurrentDateProvider
    {
        DateTime currentDate;

        public TestCurrentDateProvider(DateTime currentDate)
        {
            this.currentDate = currentDate;
        }

        public DateTime Get()
        {
            return this.currentDate;
        }

        public void AddToCurrentDate(TimeSpan toAdd)
        {
            this.currentDate = this.currentDate.Add(toAdd);
        }
    }
}