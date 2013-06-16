using System;
using System.Linq;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.local
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_after_configuring_a_channel_and_then_a_local_channel 
        : WithMessageConfigurationSubject
    {
        static Int64 message;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Channel").ForRequestReplySendingTo("Reciever")
                .OpenLocalChannel()
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_send_the_message_down_the_channel = () =>
            Server.SentMessages.First().DeserialiseTo<Int64>().ShouldEqual(message);
    }
}