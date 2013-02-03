using System.Threading.Tasks;

namespace System.Timers
{
    public class Timer
    {
        readonly double interval;
        ElapsedEventHandler elapsed;
        bool enabled;

        public Timer(double interval)
        {
            this.interval = interval;
        }

        public bool AutoReset { get; set; }

        public bool Enabled
        {
            get { return this.enabled; }
            set
            {
                this.enabled = value;
                ScheduleTask();
            }
        }

        public event ElapsedEventHandler Elapsed
        {
            add
            {
                this.elapsed += value;
            }
            remove
            {
                this.elapsed -= value;
            }
        }

        void ScheduleTask()
        {
            Task.Run(async () =>
            {
                if (!this.enabled) return;

                await Task.Delay(TimeSpan.FromMilliseconds(this.interval));

                OnElapsed();
            });
        }

        void OnElapsed()
        {
            if (this.elapsed == null) return;
            this.elapsed(this, new ElapsedEventHandlerArgs());
        }
    }
}