using System;
using System.Threading.Tasks;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications
{
    public class TestTaskStarter : ITaskStarter
    {
        public static TestTaskStarter Unlimited()
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

        public Task StartTask(Action toStart)
        {
            if (allowedInvocationCount > 0 && InvocationCount == allowedInvocationCount) return NullTask();
            this.action = toStart;

            InvocationCount++;

            if (!this.paused)
                this.action.Invoke();

            return NullTask();
        }

        static Task NullTask()
        {
            return new Task(() => { });
        }

        public void UnPause()
        {
            this.paused = false;
            if (this.action != null) this.action.Invoke();
        }

        public void Pause()
        {
            this.paused = true;
        }
    }
}