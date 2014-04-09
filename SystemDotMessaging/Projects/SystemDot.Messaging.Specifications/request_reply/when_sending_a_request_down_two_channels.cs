using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_on_two_channels : WithMessageConfigurationSubject
    {
        const string Channel1Name = "Test1";
        const string Reciever1Address = "TestRecieverAddress1";
        const string Channel2Name = "Test2";
        const string Reciever2Address = "TestRecieverAddress2";
        
        static int message;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(Channel1Name).ForRequestReplySendingTo(Reciever1Address)
                .OpenChannel(Channel2Name).ForRequestReplySendingTo(Reciever2Address)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_send_a_message_with_the_correct_to_address_through_the_first_channel = () =>
            GetServer().SentMessages.Should().Contain(m => m.GetToAddress() == BuildAddress(Reciever1Address));

        It should_send_a_message_with_the_correct_to_address_through_the_second_channel = () =>
            GetServer().SentMessages.Should().Contain(m => m.GetToAddress() == BuildAddress(Reciever2Address));
    }
}