using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.sending_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message : WithHttpConfigurationSubject
    {
        const string ServerName = "ServerName";
        const Int64 Message = 1;
       
        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ServerName)
                .OpenChannel("TestSender")
                .ForPointToPointSendingTo("TestReceiver")
                .Initialise();

        Because of = () => Bus.Send(Message);

        It should_serialise_the_message_and_send_it_as_a_put_request_to_the_message_server = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .DeserialiseTo<Int64>().ShouldBeEquivalentTo(Message);

        It should_send_a_message_with_the_correct_to_address_server_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Server.Name.ShouldBeEquivalentTo(ServerName);
    }
}
