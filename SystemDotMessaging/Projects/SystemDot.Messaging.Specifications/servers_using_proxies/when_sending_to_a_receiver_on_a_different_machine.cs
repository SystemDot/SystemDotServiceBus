using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.servers_using_proxies
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_on_a_different_machine : WithHttpConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverChannelName = "ReceiverChannelName";
        const string ReceiverServerName = "ReceiverServerName";
        const string ReceiverServerAddress = "ReceiverServerAddress";
        const string ReceiverAddress = ReceiverChannelName + "@" + ReceiverServerName;
        
        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            ServerAddressConfiguration.AddAddress(ReceiverServerName, ReceiverServerAddress);
           
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServerUsingAProxy("Server")
                .OpenChannel(SenderAddress).ForPointToPointSendingTo(ReceiverAddress)
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Channel.ShouldEqual(ReceiverChannelName);

        It should_send_a_message_with_the_correct_to_address_server_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Server.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_correct_to_address_server_address = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Server.Address.Path.ShouldEqual(ReceiverServerAddress);
    }
}