using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.Configuration;
using SystemDot.Threading;

namespace SystemDot.Messaging.Channels.Building
{
    public class StartPointBuilder<T>
    {
        private readonly IChannelStartPoint<T> startPoint;

        public StartPointBuilder(IChannelStartPoint<T> startPoint)
        {
            this.startPoint = startPoint;
        }

        public PumpBuilder<T> Pump()
        {
            var pump = new Pump<T>(MessagingEnvironment.GetComponent<IThreadPool>());
            this.startPoint.MessageProcessed += pump.InputMessage;

            return new PumpBuilder<T>(pump);
        }
    }
}