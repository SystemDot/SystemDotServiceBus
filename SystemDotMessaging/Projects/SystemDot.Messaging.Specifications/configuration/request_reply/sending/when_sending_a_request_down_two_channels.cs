using Machine.Specifications;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_down_two_channels : WithMessageConfigurationSubject
    {
        const string Channel1Name = "Test1";
        const string Reciever1Address = "TestRecieverAddress1";
        const string Channel2Name = "Test2";
        const string Reciever2Address = "TestRecieverAddress2";
        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(Channel1Name).ForRequestReplySendingTo(Reciever1Address)
                .OpenChannel(Channel2Name).ForRequestReplySendingTo(Reciever2Address)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_send_a_message_with_the_correct_to_address_through_the_first_channel = () =>
            MessageSender.SentMessages.ShouldContain(m => m.GetToAddress() == BuildAddress(Reciever1Address));

        It should_send_a_message_with_the_correct_to_address_through_the_second_channel = () =>
            MessageSender.SentMessages.ShouldContain(m => m.GetToAddress() == BuildAddress(Reciever2Address));
    }
}