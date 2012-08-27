using System;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications
{
    public class TestTaskStarter : ITaskStarter 
    {
        int invocationCount;
        readonly int allowedInvocationCount;

        public TestTaskStarter() : this(1)
        {
        }

        public TestTaskStarter(int allowedInvocationCount)
        {
            this.allowedInvocationCount = allowedInvocationCount;
        }

        public void StartTask(Action action)
        {
            if (invocationCount == allowedInvocationCount) return;
            
            invocationCount++;
            action.Invoke();
        }
    }
}