using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace SystemDot.Parallelism
{
    public class TaskLooper : ITaskLooper
    {
        readonly ITaskStarter starter;
        readonly List<Func<Task>> looping;

        public TaskLooper(ITaskStarter starter)
        {
            Contract.Requires(starter != null);

            this.starter = starter;
            this.looping = new List<Func<Task>>();
        }

        public void RegisterToLoop(Func<Task> toLoop)
        {
            Contract.Requires(toLoop != null);
            
            this.looping.Add(toLoop);
        }

        public void Start()
        {
            looping.ForEach(l => this.starter.StartTask(() => PerformLoop(l)));
        }

        public void PerformLoop(Func<Task> toLoop)
        {
            toLoop().ContinueWith(_ => PerformLoop(toLoop));
        }
    }
}