using System;
using System.Collections.Generic;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications
{
    public class TestTaskRepeater : ITaskRepeater
    {
        readonly List<Action> toRun;
        
        public bool Started { get; private set; }

        public TestTaskRepeater()
        {
            this.toRun = new List<Action>();
        }

        public void Register(TimeSpan delay, Action toLoop)
        {
            this.toRun.Add(toLoop);
            if (this.Started) this.toRun.ForEach(r => r());
        }

        public void Start()
        {
            Started = true;
            this.toRun.ForEach(r => r());
        }
    }
}