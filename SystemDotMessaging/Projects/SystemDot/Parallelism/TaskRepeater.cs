using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Parallelism
{
    public class TaskRepeater : ITaskRepeater
    {
        private class RegisteredAction
        {
            public TimeSpan Delay { get; private set; }

            public Action ToLoop { get; private set; }

            public RegisteredAction(TimeSpan delay, Action toLoop)
            {
                Delay = delay;
                ToLoop = toLoop;
            }
        }

        readonly ITaskScheduler scheduler;
        readonly List<RegisteredAction> registeredActions;
        
        public TaskRepeater(ITaskScheduler scheduler)
        {
            this.scheduler = scheduler;
            this.registeredActions = new List<RegisteredAction>();
        }

        public void Register(TimeSpan delay, Action toLoop)
        {
            Contract.Requires(delay != null);
            Contract.Requires(toLoop != null);

            this.registeredActions.Add(new RegisteredAction(delay, toLoop));
        }

        public void Start()
        {
            this.registeredActions.ForEach(a => this.scheduler.ScheduleTask(TimeSpan.FromMilliseconds(1), () => LoopTask(a)));
        }

        void LoopTask(RegisteredAction action)
        {
            action.ToLoop();
            this.scheduler.ScheduleTask(action.Delay, () => LoopTask(action));
        }
    }

}