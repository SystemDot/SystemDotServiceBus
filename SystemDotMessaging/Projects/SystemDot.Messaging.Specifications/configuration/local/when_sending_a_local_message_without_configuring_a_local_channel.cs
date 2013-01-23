using System;
using SystemDot.Messaging.Channels.Local;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.local
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_local_message_without_configuring_a_local_channel : WithNoRepeaterMessageConfigurationSubject
    {
        static IBus bus;
        static Exception exception;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Channel").ForRequestReplyRecieving()
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => bus.SendLocal(new object()));

        It should_fail = () => exception.ShouldBeOfType<NoLocalChannelConfiguredException>();
    }
}