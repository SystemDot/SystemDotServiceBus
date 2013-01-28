using System;
using System.Linq;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_down_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        
        static InMemoryChangeStore inMemoryChangeStore; 
        static IBus bus;
        static int message;
        
        Establish context = () =>
        {
            inMemoryChangeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());
            ConfigureAndRegister<MessageCacheFactory>(new MessageCacheFactory(inMemoryChangeStore));
            
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_send_a_message_with_the_correct_to_address = () =>
            MessageSender.SentMessages.First().GetToAddress().ShouldEqual(BuildAddress(RecieverAddress));

        It should_send_a_message_with_the_correct_from_address = () =>
            MessageSender.SentMessages.First().GetFromAddress().ShouldEqual(BuildAddress(ChannelName));

        It should_send_a_message_with_the_correct_content = () =>
            MessageSender.SentMessages.First().DeserialiseTo<int>().ShouldEqual(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            MessageSender.SentMessages.First().GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    MessageSender.SentMessages.First().Id,
                    BuildAddress(ChannelName),
                    PersistenceUseType.RequestSend));

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           MessageSender.SentMessages.First().GetSourcePersistenceId()
               .ShouldEqual(MessageSender.SentMessages.First().GetPersistenceId());

        It should_mark_the_message_with_the_time_the_message_is_sent = () =>
            MessageSender.SentMessages.First().GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_message_with_the_amount_of_times_the_message_has_been_sent = () =>
            MessageSender.SentMessages.First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            MessageSender.SentMessages.First().GetSequence().ShouldEqual(1);

        It should_not_persist_the_message = () =>
             inMemoryChangeStore
                .GetMessages(PersistenceUseType.RequestSend, BuildAddress(ChannelName))
                .ShouldBeEmpty();

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }

}