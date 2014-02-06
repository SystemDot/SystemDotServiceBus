using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.point_to_point
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_messaging_after_a_message_was_sent_but_not_acknowledged_on_a_durable_channel
        : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";
        const int Message1 = 1;
        const int Message2 = 2;

        static ChangeStore changeStore;
        static List<MessageLoadedToCache> messagesLoadedToCacheEvents;

        Establish context = () =>
        {
            messagesLoadedToCacheEvents = new List<MessageLoadedToCache>();

            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister(changeStore);

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForPointToPointSendingTo(ReceiverAddress).WithDurability()
                .Initialise();

            Bus.Send(Message1);
            Bus.Send(Message2);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));

            Messenger.RegisterHandler<MessageLoadedToCache>(e => messagesLoadedToCacheEvents.Add(e));
        };

        Because of = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForPointToPointSendingTo(ReceiverAddress).WithDurability()
                .Initialise();

        It should_notify_that_the_first_sent_message_was_loaded_into_the_cache = () =>
            messagesLoadedToCacheEvents.ShouldContain(m =>
                m.CacheAddress == BuildAddress(SenderAddress)
                && m.UseType == PersistenceUseType.PointToPointSend
                && m.Message == GetServer().SentMessages.First());

        It should_notify_that_the_second_sent_message_was_loaded_into_the_cache = () =>
            messagesLoadedToCacheEvents.ShouldContain(m =>
                m.CacheAddress == BuildAddress(SenderAddress)
                && m.UseType == PersistenceUseType.PointToPointSend
                && m.Message == GetServer().SentMessages.ElementAt(1));
    }
}