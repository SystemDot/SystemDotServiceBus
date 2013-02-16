using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_time_expired_request_down_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplySendingTo(RecieverAddress)
                    .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromMinutes(0)))
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_not_send_the_message = () => Server.SentMessages.ShouldBeEmpty();
    }
}