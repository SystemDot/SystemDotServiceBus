using System;
using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_time_expired_request_down_a_channel_with_an_action_to_execute_on_expiry_set 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static bool expiryActionExecuted;

        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ChannelName)
            .ForRequestReplySendingTo(RecieverAddress)
            .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromMinutes(0)), () => expiryActionExecuted = true)
            .Initialise();

        Because of = () => Bus.Send(1);

        It should_execute_the_expiry_action = () => expiryActionExecuted.ShouldBeTrue();

        It should_not_send_the_message = () => Server.SentMessages.ShouldBeEmpty();
    }
}