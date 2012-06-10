using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Recieving;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_consumption
{
    public class when_broadcasting_a_message_from_a_pipe_to_two_consumers
    {
        static Pipe pipe;
        static TestConsumer<string> consumer1;
        static TestConsumer<string> consumer2;

        Establish context = () =>
        {
            pipe = new Pipe();
            var broadcaster = new ConsumerMessageBroadcaster(pipe);
       
            consumer1 = new TestConsumer<string>();
            consumer2 = new TestConsumer<string>();
       
            broadcaster.RegisterConsumer(consumer1);
            broadcaster.RegisterConsumer(consumer2);
        };

        Because of = () => pipe.Publish("Test");

        It should_consume_the_message_in_the_first_consumer = () => consumer1.ConsumedMessage.ShouldEqual("Test");

        It should_consume_the_message_in_the_second_consumer = () => consumer2.ConsumedMessage.ShouldEqual("Test");
    }
}