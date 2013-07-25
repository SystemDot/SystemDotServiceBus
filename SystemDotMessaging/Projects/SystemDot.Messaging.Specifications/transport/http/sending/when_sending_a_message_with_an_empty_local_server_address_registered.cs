using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_an_empty_local_server_address_registered : WithHttpConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";
        const string ServerName = "ServerName";

        Establish context = () =>
        {
            WebRequestor.ExpectAddress(ServerName, Environment.MachineName);
            ServerAddressConfiguration.AddAddress(ServerName, string.Empty);

            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(ServerName)
                .OpenChannel(SenderAddress).ForPointToPointSendingTo(ReceiverAddress)
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_send_a_message_with_a_from_server_address_set_to_the_local_machine_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().Route.Server.Address.Path.ShouldEqual(Environment.MachineName);
    }
}