using System;
using System.Threading;

namespace SystemDot.Messaging.Messages.Processing
{
    public class ThreadLocalChannelReserve
    {
        readonly ThreadLocal<Guid> reservedChannel = new ThreadLocal<Guid>();

        public Guid ReservedChannel
        {
            get { return reservedChannel.Value; }
            set { reservedChannel.Value = value; }
        }
    }
}