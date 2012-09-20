using System;

namespace SystemDot
{
    public class CurrentDateProvider : ICurrentDateProvider
    {
        public DateTime Get()
        {
            return DateTime.Now;
        }
    }
}