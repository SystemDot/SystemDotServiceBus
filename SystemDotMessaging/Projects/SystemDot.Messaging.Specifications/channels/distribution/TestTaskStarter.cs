using System;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications.messages.distribution
{
    public class TestTaskStarter : ITaskStarter 
    {
        public void StartTask(Action action)
        {
            action.Invoke();
        }
    }
}