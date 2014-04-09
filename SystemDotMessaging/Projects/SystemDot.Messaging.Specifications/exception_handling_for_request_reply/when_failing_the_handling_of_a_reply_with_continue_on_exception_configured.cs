using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.exception_handling_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_a_reply_with_continue_on_exception_configured : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";
        static Exception exception;

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForRequestReplySendingTo(ReceiverAddress)
                .OnException().ContinueProcessingMessages()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<Int64>()))
                .Initialise();

        Because of = () => exception = Catch.Exception(
            () => GetServer().ReceiveMessage(
                new MessagePayload()
                    .SetMessageBody(1)
                    .SetFromChannel(ReceiverAddress)
                    .SetToChannel(SenderAddress)
                    .SetChannelType(PersistenceUseType.ReplySend)
                    .Sequenced()));

        It should_not_throw_an_exception = () => exception.Should().BeNull();
    }
}