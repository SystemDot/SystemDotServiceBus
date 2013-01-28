using SystemDot.Ioc;
using SystemDot.Messaging.Channels.PointToPoint;
using SystemDot.Messaging.Channels.PointToPoint.Builders;
using SystemDot.Messaging.Configuration.RequestReply;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PointToPointComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<PointToPointSendChannelBuilder, PointToPointSendChannelBuilder>();
            iocContainer.RegisterInstance<PointToPointReceiveChannelBuilder, PointToPointReceiveChannelBuilder>();
        }
    }
}