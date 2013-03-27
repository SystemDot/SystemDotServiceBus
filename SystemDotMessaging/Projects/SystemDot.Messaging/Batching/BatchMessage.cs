using System.Collections.Generic;

namespace SystemDot.Messaging.Batching
{
    public class BatchMessage
    {
        public List<object> Messages { get; set; }

        public BatchMessage()
        {
            this.Messages = new List<object>();
        }
    }
}