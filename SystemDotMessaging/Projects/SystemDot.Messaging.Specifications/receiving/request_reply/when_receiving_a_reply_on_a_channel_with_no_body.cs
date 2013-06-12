using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.receiving.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_on_a_channel_with_no_body : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static MessagePayload payload;
        static Exception exception;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                
                .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
                .Initialise();

            payload = new MessagePayload();
            payload.SetToAddress(BuildAddress(ChannelName));
        };

        Because of = () => exception = Catch.Exception(() => Server.ReceiveMessage(payload));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}