using System.Collections.Generic;

namespace SystemDot.Messaging.Aggregation
{
    public class AggregateMessage
    {
        public List<object> Messages { get; set; }

        public AggregateMessage()
        {
            this.Messages = new List<object>();
        }
    }
}