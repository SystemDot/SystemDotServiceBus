using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Channels.Remote;
using SystemDot.Messaging.MessageTransportation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.long_polling
{
    [Subject("Long polling")]
    public class when_recieveing_messages_in_the_response_from_a_long_poll_request
    {
        static TestWebRequestor requestor;
        static BinaryFormatter formatter;
        static LongPollReciever reciever;
        static List<MessagePayload> messagePayloads;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;
            
        Establish context = () =>
        {
            messagePayloads = new List<MessagePayload>();
            messagePayload1 = new MessagePayload(new Address("Address1"));
            messagePayload2 = new MessagePayload(new Address("Address2"));
            
            formatter = new BinaryFormatter();
            requestor = new TestWebRequestor();
            requestor.ResponseStream.Serialise(new List<MessagePayload> { messagePayload1, messagePayload2 }, formatter);

            reciever = new LongPollReciever(new Address("Address"), requestor, formatter);
            reciever.MessageProcessed += payload => messagePayloads.Add(payload);
        };

        Because of = () => reciever.PerformWork();

        It should_output_the_first_recieved_message = () =>
            messagePayloads.First().Address.ShouldEqual(messagePayload1.Address);

        It should_output_the_second_recieved_message = () =>
             messagePayloads.Last().Address.ShouldEqual(messagePayload2.Address);
    }
}