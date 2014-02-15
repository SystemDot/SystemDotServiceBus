using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.UnitOfWork;
using SystemDot.Serialisation;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.unitofwork
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_a_unit_of_work_specified_and_an_exception_occurs :
        WithMessageConfigurationSubject
    {
        private const string ReceiverChannel = "ReceiverChannel";
        private const long Message = 1;

        private static MessagePayload payload;
        private static FailingMessageHandler<long> handler;
        private static Exception exception;

        private Establish context = () =>
        {
            handler = new FailingMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverChannel)
                .ForPointToPointReceiving()
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("SenderChannel")
                .SetToChannel(ReceiverChannel)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();
        };

        Because of = () => exception = Catch.Exception(() => GetServer().ReceiveMessage(payload));

        It should_throw_a_unit_of_work_exception_containing_the_correct_exception_text = 
            () => exception.Message.Should().Contain("Unit of work failed for message: " 
                + new JsonSerialiser().SerialiseToString(Message));
    }
}