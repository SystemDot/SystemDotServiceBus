using System;
using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_time_expired_request_down_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        
        static int message;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplySendingTo(RecieverAddress)
                    .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromMinutes(0)))
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_not_send_the_message = () => Server.SentMessages.ShouldBeEmpty();
    }
}