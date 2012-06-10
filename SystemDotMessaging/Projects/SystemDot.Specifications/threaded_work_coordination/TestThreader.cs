using System;
using System.Collections.Generic;
using SystemDot.Threading;

namespace SystemDot.Specifications.threaded_work_coordination
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