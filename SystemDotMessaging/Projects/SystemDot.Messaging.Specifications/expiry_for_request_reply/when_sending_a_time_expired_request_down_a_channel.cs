using System;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.expiry_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_time_expired_request_down_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ChannelName)
            .ForRequestReplySendingTo(RecieverAddress)
            .ExpireMessages().After(TimeSpan.FromMinutes(0))
            .Initialise();

        Because of = () => Bus.Send(1);

        It should_not_send_the_message = () => GetServer().SentMessages.Should().BeEmpty();
    }
}