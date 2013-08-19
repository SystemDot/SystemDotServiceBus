using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.servers_using_proxies
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_to_a_server_that_uses_a_proxy : WithHttpConfigurationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("SenderServer")
                .OpenChannel("SenderChannel")
                .ForPointToPointSendingTo("ReceiverChannel@ReceiverServer.MachineName")
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_send_a_message_with_the_to_address_server_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Server.Name.ShouldEqual("ReceiverServer");

        It should_send_a_message_with_the_from_address_server_machine_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Server.MachineName.ShouldEqual("MachineName");
    }
}