using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_with_a_unit_of_work_setup : WithPublisherSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static MessagePayload payload;
        static TestUnitOfWork unitOfWork;
        
        Establish context = () =>
        {
            unitOfWork = new TestUnitOfWork();
            ConfigureAndRegister<TestUnitOfWorkFactory>(new TestUnitOfWorkFactory(unitOfWork));

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForSubscribingTo(PublisherName)
                    .WithUnitOfWork<TestUnitOfWorkFactory>()
                .Initialise();

            var handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = new MessagePayload().MakeReceiveable(1, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            payload.SetFirstSequence(1);
            payload.SetSequence(1); 
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_begin_the_unit_of_work = () => unitOfWork.HasBegun().ShouldBeTrue();
    }
}