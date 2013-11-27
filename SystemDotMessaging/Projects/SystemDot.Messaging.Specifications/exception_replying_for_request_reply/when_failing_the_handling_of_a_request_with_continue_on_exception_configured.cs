using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply.ExceptionHandling;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.exception_replying_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_a_request_with_continue_on_exception_configured : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForRequestReplyReceiving()
                .OnException().ContinueProcessingMessages()
                .WithDurability()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<Int64>()))
                .Initialise();

        Because of = () => GetServer().ReceiveMessage(
            new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderAddress")
                .SetToChannel(ReceiverAddress)
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced());

        It should_reply_with_an_exception_occurred_message = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<ExceptionOccured>().ShouldNotBeNull();
    }
}
