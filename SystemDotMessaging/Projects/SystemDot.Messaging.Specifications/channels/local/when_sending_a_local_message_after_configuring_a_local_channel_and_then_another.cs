using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.local
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_local_message_after_configuring_a_local_channel_and_then_another 
        : WithMessageConfigurationSubject
    {
        
        static Exception exception;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenLocalChannel()
                .OpenChannel("Channel").ForRequestReplyRecieving()
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => Bus.SendLocal(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}