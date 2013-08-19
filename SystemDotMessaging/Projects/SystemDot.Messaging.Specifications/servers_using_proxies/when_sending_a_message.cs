using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.servers_using_proxies
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message : WithHttpConfigurationSubject
    {
        const string Server = "Server";
        const string ChannelAddress = "TestSender";
        const string RecieverAddress = "TestReceiver";
        const Int64 Message = 1;

        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServerUsingAProxy(Server)
                .OpenChannel(ChannelAddress)
                    .ForPointToPointSendingTo(RecieverAddress)
                .Initialise();
        };

        Because of = () => Bus.Send(Message);

        It should_serialise_the_message_and_send_it_as_a_put_request_to_the_message_server = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().DeserialiseTo<Int64>().ShouldEqual(Message);

        It should_send_a_message_with_the_to_address_channel_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Channel.ShouldEqual(RecieverAddress);

        It should_send_a_message_with_the_to_address_server_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Server.Name.ShouldEqual(Server);

        It should_send_a_message_with_the_from_address_channel_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetFromAddress().Channel.ShouldEqual(ChannelAddress);

        It should_send_a_message_with_the_from_address_server_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetFromAddress().Server.Name.ShouldEqual(Server);

        It should_send_a_message_with_the_from_address_server_machine_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetFromAddress().Server.MachineName.ShouldEqual(Environment.MachineName);
    }
}
