using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Authentication.Expiry
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

        public void Track(MessageServer server, AuthenticationSession toTrack)
        {
            Contract.Requires(toTrack != null);
            Contract.Requires(server != null);

            if (toTrack.NeverExpires()) return;
            ScheduleDecache(server, toTrack);
        }

        void ScheduleDecache(MessageServer server, AuthenticationSession toTrack)
        {
            taskScheduler.ScheduleTask(GetDecacheTime(toTrack), () => ExpireSession(server, toTrack));
        }

        static void ExpireSession(MessageServer server, AuthenticationSession toTrack)
        {
            Logger.Debug("Expiring authentication session {0}", toTrack.Id);
            Messenger.Send(new AuthenticationSessionExpired {Server = server, Session = toTrack});
        }

        TimeSpan GetDecacheTime(AuthenticationSession toTrack)
        {
            return toTrack.GetExpiresOn().Subtract(systemTime.GetCurrentDate());
        }
    }
}