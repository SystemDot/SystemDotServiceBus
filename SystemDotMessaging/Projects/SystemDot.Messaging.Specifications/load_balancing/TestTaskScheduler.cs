using System;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications.load_balancing
{
    public class TestTaskScheduler : ITaskScheduler
    {
        Action taskToRun;
        TimeSpan delayToRunTaskAfter;
        
        public void ScheduleTask(TimeSpan delay, Action task)
        {
            this.taskToRun = task;
            this.delayToRunTaskAfter = delay;
        }

        public void PassTime(TimeSpan toPass)
        {
            if (toPass == this.delayToRunTaskAfter)
                this.taskToRun();
        }
    }
}