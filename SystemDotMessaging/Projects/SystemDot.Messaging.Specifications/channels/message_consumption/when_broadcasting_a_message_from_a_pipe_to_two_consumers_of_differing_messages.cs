using SystemDot.Messaging.Channels;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.message_consumption
{
    public class when_broadcasting_a_message_from_a_pipe_to_two_consumers_of_differing_messages
    {
        static Pipe channel;
        static TestConsumer<string> consumer1;
        static TestConsumer<int> consumer2;

        Establish context = () =>
        {
            channel = new Pipe();
            var broadcaster = new ConsumerMessageBroadcaster(channel);
            
            consumer1 = new TestConsumer<string>();
            consumer2 = new TestConsumer<int>();

            broadcaster.RegisterConsumer(consumer1);
            broadcaster.RegisterConsumer(consumer2);            
        };

        Because of = () => channel.Publish("Test");

        It should_consume_the_message_in_the_first_consumer = () => consumer1.ConsumedMessage.ShouldEqual("Test");
    }
}