using System;
using System.Collections.Generic;
using SystemDot.Messaging.Threading;

namespace SystemDot.Messaging.Specifications.threaded_distribution
{
    public class TestThreader : IThreader 
    {
        readonly List<Action> starts;

        public TestThreader()
        {
            this.starts = new List<Action>();
        }

        public void Start(Action action)
        {
            this.starts.Add(action);
        }

        public void RunStarts()
        {
            this.starts.ForEach(a => a.Invoke());
        }
    }
}