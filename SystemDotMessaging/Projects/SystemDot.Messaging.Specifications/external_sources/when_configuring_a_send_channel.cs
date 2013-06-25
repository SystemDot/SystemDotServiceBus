using System.Collections.Generic;
using SystemDot.Messaging.PointToPoint.Builders;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.external_sources
{
    [Subject(sending.SpecificationGroup.Description)]
    public class when_configuring_a_send_channel : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static List<PointToPointSendChannelBuilt> channelBuiltEvents;

        Establish context = () =>
        {
            channelBuiltEvents = new List<PointToPointSendChannelBuilt>();
            Messenger.Register<PointToPointSendChannelBuilt>(m => channelBuiltEvents.Add(m));
        };

        Because of = () => Messaging.Configuration.Configure.Messaging()
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
