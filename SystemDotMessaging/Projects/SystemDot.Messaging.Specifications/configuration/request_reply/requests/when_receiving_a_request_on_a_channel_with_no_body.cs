using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_channel_with_no_body : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        
        static MessagePayload payload;
        static Exception exception;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplyRecieving()
                .Initialise();

            payload = new MessagePayload();
            payload.SetFromAddress(BuildAddress("TestSender"));
            payload.SetToAddress(BuildAddress(ChannelName));
        };

        Because of = () => exception = Catch.Exception(() => MessageReciever.ReceiveMessage(payload));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}