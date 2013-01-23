using System;
using SystemDot.Messaging.Channels.Packaging;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_a_request_on_a_channel_with_no_body : WithMessageConfigurationSubject
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

        Because of = () => exception = Catch.Exception(() => MessageReciever.RecieveMessage(payload));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}