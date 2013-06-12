using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_channel : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";

        static List<PointToPointReceiveChannelBuilt> channelBuiltEvents;

        Establish context = () =>
        {
            channelBuiltEvents = new List<PointToPointReceiveChannelBuilt>();
            Messenger.Register<PointToPointReceiveChannelBuilt>(m => channelBuiltEvents.Add(m));
        };

        Because of = () => Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForPointToPointReceiving()
                .Initialise();

        It should_notify_that_the_channel_was_built = () =>
            channelBuiltEvents.First().Address.ShouldEqual(BuildAddress(ReceiverAddress));
    }
}
