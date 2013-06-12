using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_with_named_server_and_proxy_but_with_no_addresses_registered 
        : WithHttpConfigurationSubject
    {
        const string ChannelName = "ChannelName";
        const string ReceiverChannelName = "ReceiverChannelName";
        const string ReceiverServerName = "ReceiverServerName";
        const string RemoteProxyName = "RemoteProxyName";

        static string receiverName;

        Establish context = () =>
        {
            receiverName = ReceiverChannelName + "@" + ReceiverServerName + "." + RemoteProxyName;

            WebRequestor.ExpectAddress(RemoteProxyName, Environment.MachineName);

            Messaging.Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("ServerName")
                .OpenChannel(ChannelName).ForPointToPointSendingTo(receiverName)
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_send_a_message_with_the_expected_to_address_server_name_set = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_expected_to_address_remote_proxy_name_set = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(RemoteProxyName);
    }
}