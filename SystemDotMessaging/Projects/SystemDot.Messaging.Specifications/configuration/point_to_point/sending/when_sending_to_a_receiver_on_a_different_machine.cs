using System;
using System.Linq;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.point_to_point.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_on_a_different_machine : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverChannelName = "TestReceiverChannel";
        const string ReceiverServerName = "TestReceiverServer";
        const string ReceiverName = ReceiverChannelName + "@" + ReceiverServerName;
        
        static IBus bus;
        static int message;
        
        Establish context = () =>
        {    
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
            MessageServer.SentMessages.First().GetToAddress()
                .Channel.ShouldEqual(ReceiverChannelName);

        It should_send_a_message_with_the_correct_to_address_server_name = () =>
           MessageServer.SentMessages.First().GetToAddress()
               .ServerPath.LocatedAt.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_to_address_set_to_the_local_message_server_name = () =>
            MessageServer.SentMessages.First().GetToAddress()
                .ServerPath.RoutedVia.Name.ShouldEqual(Environment.MachineName);
    }
}