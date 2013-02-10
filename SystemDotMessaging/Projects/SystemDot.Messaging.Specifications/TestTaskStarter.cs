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
        bool paused;
        Action action;

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
            this.action = action;

            InvocationCount++;
            
            if (this.paused) return;

            this.action.Invoke();
        }

        public void UnPause()
        {
            this.paused = false;
            this.action.Invoke();
        }

        public void Pause()
        {
            this.paused = true;
        }
    }
}