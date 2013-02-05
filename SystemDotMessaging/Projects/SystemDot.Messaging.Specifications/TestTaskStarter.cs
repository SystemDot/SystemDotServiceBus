using System;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications
{
    public class TestTaskStarter : ITaskStarter
    {
        public static ITaskStarter Unlimited()
        {
            return new TestTaskStarter(0);
        }

        readonly int allowedInvocationCount;

        public int InvocationCount { get; private set; }

        public TestTaskStarter()
            : this(1)
        {
        }

        public TestTaskStarter(int allowedInvocationCount)
        {
            this.allowedInvocationCount = allowedInvocationCount;
        }

        public void StartTask(Action action)
        {
            if (allowedInvocationCount > 0 && InvocationCount == allowedInvocationCount) return;

            InvocationCount++;
            action.Invoke();
        }
    }
}