using System;
using System.Diagnostics.Contracts;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionExpirer
    {
        readonly ISystemTime systemTime;
        readonly ITaskScheduler taskScheduler;

        public AuthenticationSessionExpirer(ISystemTime systemTime, ITaskScheduler taskScheduler)
        {
            Contract.Requires(systemTime != null);
            Contract.Requires(taskScheduler != null);

            this.systemTime = systemTime;
            this.taskScheduler = taskScheduler;
        }

        public void Track(AuthenticationSession toTrack)
        {
            Contract.Requires(toTrack != null);

            if (toTrack.NeverExpires()) return;
            ScheduleDecache(toTrack);
        }

        void ScheduleDecache(AuthenticationSession toTrack)
        {
            taskScheduler.ScheduleTask(GetDecacheTime(toTrack), () => Messenger.Send(new SessionExpired(toTrack)));
        }

        TimeSpan GetDecacheTime(AuthenticationSession toTrack)
        {
            return toTrack.GracePeriodEndOn.Subtract(systemTime.GetCurrentDate());
        }
    }
}