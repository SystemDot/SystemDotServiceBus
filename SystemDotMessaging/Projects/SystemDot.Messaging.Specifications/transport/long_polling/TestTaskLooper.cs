using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    public class TestTaskLooper : ITaskLooper
    {
        readonly List<Func<Task>> looping;

        public TestTaskLooper()
        {
            this.looping = new List<Func<Task>>();
        }

        public void RegisterToLoop(Func<Task> toLoop)
        {
            this.looping.Add(toLoop);
        }

        public void Start()
        {
            this.looping.ForEach(l => l());
        }
    }
}