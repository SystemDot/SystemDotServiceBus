using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.exception_handling_for_point_to_point_receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_a_request_with_continue_on_exception_configured : WithMessageConfigurationSubject
    {
        const string ReceiverChannel = "ReceiverChannel";
        static Exception exception;

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverChannel)
                .ForPointToPointReceiving()
                .OnException().ContinueProcessingMessages()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<Int64>()))
                .Initialise();

        Because of = () => exception = Catch.Exception(
            () => GetServer().ReceiveMessage(
                new MessagePayload()
                    .SetMessageBody(1)
                    .SetFromChannel("SenderChannel")
                    .SetToChannel(ReceiverChannel)
                    .SetChannelType(PersistenceUseType.PointToPointReceive)
                    .Sequenced()));

        It should_not_throw_an_exception = () => exception.Should().BeNull();
    }
}
