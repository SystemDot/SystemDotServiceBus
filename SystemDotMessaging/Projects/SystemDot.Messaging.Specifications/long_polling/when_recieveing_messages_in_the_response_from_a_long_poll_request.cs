using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Messaging.Recieving;
using SystemDot.Messaging.Specifications.message_serving;
using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.long_polling
{
    [Subject("Long polling")]
    public class when_recieveing_messages_in_the_response_from_a_long_poll_request
    {
        static TestWebRequestor requestor;
        static Pipe<MessagePayload> pipe;
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
            messagePayloads.Add(messagePayload1);
            messagePayloads.Add(messagePayload2);

            pipe = new Pipe<MessagePayload>();
            pipe.ItemPushed += payload => messagePayloads.Add(payload);
            
            formatter = new BinaryFormatter();
            requestor = new TestWebRequestor(messagePayloads);

            reciever = new LongPollReciever(new Address("Address"), pipe, requestor, formatter);
        };

        Because of = () => reciever.PerformWork();

        It should_recieve_the_first_message_onto_the_pipe = () =>
            requestor.ResponseStream.Deserialise<IEnumerable<MessagePayload>>(formatter)
                .First().Address.ShouldEqual(messagePayload1.Address);
        
        It should_recieve_the_second_message_onto_the_pipe = () =>
             requestor.ResponseStream.Deserialise<IEnumerable<MessagePayload>>(formatter)
                .Last().Address.ShouldEqual(messagePayload2.Address);
    }
}