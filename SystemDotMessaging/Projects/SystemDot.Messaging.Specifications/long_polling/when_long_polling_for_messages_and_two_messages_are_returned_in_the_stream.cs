using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.LongPolling;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.long_polling
{
    [Subject("Long polling")]
    public class when_long_polling_for_messages_and_two_messages_are_returned_in_the_stream
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
            messagePayload1 = new MessagePayload();
            messagePayload1.SetToAddress(new Address("Address1"));
            messagePayload2 = new MessagePayload();
            messagePayload2.SetToAddress(new Address("Address2"));
            
            var address = new Address("Address");
            formatter = new BinaryFormatter();
            requestor = new TestWebRequestor(formatter);
            requestor.AddMessages(address.Url, messagePayload1, messagePayload2);

            reciever = new LongPollReciever(requestor, formatter);
            reciever.RegisterUrl(address);
            reciever.MessageProcessed += payload => messagePayloads.Add(payload);
        };

        Because of = () => reciever.PerformWork();

        It should_output_the_first_recieved_message = () =>
            messagePayloads.First().GetToAddress().ShouldEqual(messagePayload1.GetToAddress());

        It should_output_the_second_recieved_message = () =>
             messagePayloads.Last().GetToAddress().ShouldEqual(messagePayload2.GetToAddress());
    }
}