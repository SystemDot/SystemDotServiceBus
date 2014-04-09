using System.Collections.Generic;
using SystemDot.Messaging.Handling.Actions;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Simple;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.external_sources
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_send_channel : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static List<PointToPointSendChannelBuilt> channelBuiltEvents;
        static ActionSubscriptionToken<PointToPointSendChannelBuilt> token;

        Establish context = () =>
        {
            channelBuiltEvents = new List<PointToPointSendChannelBuilt>();
            token = Messenger.RegisterHandler<PointToPointSendChannelBuilt>(m => channelBuiltEvents.Add(m));
        };

        Because of = () => Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForPointToPointSendingTo(ReceiverAddress)
                .Initialise();

        It should_notify_that_the_channel_was_built = () =>
            channelBuiltEvents.Should().Contain(m => 
                m.SenderAddress == BuildAddress(SenderAddress)
                && m.ReceiverAddress == BuildAddress(ReceiverAddress));
    }
}
