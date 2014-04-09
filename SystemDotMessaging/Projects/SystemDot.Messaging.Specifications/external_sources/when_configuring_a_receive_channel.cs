using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Handling.Actions;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Simple;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.external_sources
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_receive_channel : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";

        static List<PointToPointReceiveChannelBuilt> channelBuiltEvents;
        static ActionSubscriptionToken<PointToPointReceiveChannelBuilt> token;

        Establish context = () =>
        {
            channelBuiltEvents = new List<PointToPointReceiveChannelBuilt>();
            token = Messenger.RegisterHandler<PointToPointReceiveChannelBuilt>(m => channelBuiltEvents.Add(m));
        };

        Because of = () => Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForPointToPointReceiving()
                .Initialise();

        It should_notify_that_the_channel_was_built = () =>
            channelBuiltEvents.First().Address.ShouldBeEquivalentTo(BuildAddress(ReceiverAddress));
    }
}
