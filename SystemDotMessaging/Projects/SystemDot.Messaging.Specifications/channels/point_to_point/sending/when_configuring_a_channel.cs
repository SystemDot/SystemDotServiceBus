using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_channel : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static List<PointToPointSendChannelBuilt> channelBuiltEvents;

        Establish context = () =>
        {
            channelBuiltEvents = new List<PointToPointSendChannelBuilt>();
            Messenger.Register<PointToPointSendChannelBuilt>(m => channelBuiltEvents.Add(m));
        };

        Because of = () => Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForPointToPointSendingTo(ReceiverAddress)
                .Initialise();

        It should_notify_that_the_channel_was_built = () =>
            channelBuiltEvents.ShouldContain(m => 
                m.SenderAddress == BuildAddress(SenderAddress)
                && m.ReceiverAddress == BuildAddress(ReceiverAddress));
    }
}
