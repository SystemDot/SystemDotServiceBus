using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Simple;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications
{
    public class TestTaskScheduler : ITaskScheduler
    {
        readonly TestSystemTime systemTime;
        readonly List<Task> tasks;
        int schedulesExecuted;

        public int SchedulesPermitted { get; set; }

        public TestTaskScheduler(TestSystemTime systemTime)
        {
            this.systemTime = systemTime;
            tasks = new List<Task>();

            Messenger.RegisterHandler<TestSystemTimeAdvanced>(_ => RunAwaitingTasks());
        }

        void RunAwaitingTasks()
        {
            tasks
                .Where(t => t.ExecuteAt <= systemTime.GetCurrentDate())
                .ToList()
                .ForEach(t => t.Run());

            tasks
                .RemoveAll(t => t.ExecuteAt <= systemTime.GetCurrentDate());
        }

        public void ScheduleTask(TimeSpan delay, Action task)
        {
            if (SchedulesPermitted > 0 && ++schedulesExecuted > SchedulesPermitted) return;

            tasks.Add(new Task(task, systemTime.GetCurrentDate().Add(delay)));
        }

        class Task
        {
            readonly Action toRun;

            public DateTime ExecuteAt { get; private set; }

            public Task(Action toRun, DateTime executeAt)
            {
                this.toRun = toRun;
                ExecuteAt = executeAt;
            }

            public void Run()
            {
                toRun.Invoke();
            }
        }
    }
}