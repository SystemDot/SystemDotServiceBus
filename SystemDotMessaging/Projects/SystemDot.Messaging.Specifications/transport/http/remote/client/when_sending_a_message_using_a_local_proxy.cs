using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_using_a_local_proxy : WithHttpConfigurationSubject
    {
        const string Server = "Server";
        const string Proxy = "Proxy";
        const string ChannelAddress = "TestSender";
        const string RecieverAddress = "TestReceiver";

        static Int64 message;

        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            WebRequestor.ExpectAddress(Proxy, Environment.MachineName);

            Messaging.Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServerUsingAProxy(Server, Proxy)
                .OpenChannel(ChannelAddress)
                .ForPointToPointSendingTo(RecieverAddress)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_serialise_the_message_and_send_it_as_a_put_request_to_the_message_server = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .DeserialiseTo<Int64>().ShouldEqual(message);

        It should_send_a_message_with_the_to_address_channel_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Channel.ShouldEqual(RecieverAddress);

        It should_send_a_message_with_the_to_address_server_set_to_the_remote_client = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(Server);

        It should_send_a_message_with_the_to_address_proxy_set_to_the_proxy = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(Proxy);

        It should_send_a_message_with_the_from_address_channel_name_set_correctly = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().Channel.ShouldEqual(ChannelAddress);

        It should_send_a_message_with_the_from_address_server_set_to_the_remote_client = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Server.Name.ShouldEqual(Server);

        It should_send_a_message_with_the_from_address_proxy_set_to_the_proxy = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Proxy.Name.ShouldEqual(Proxy);
    }
}
