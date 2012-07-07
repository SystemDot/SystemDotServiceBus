using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Transport.Http;
using SystemDot.Messaging.Transport.Http.LongPolling;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    [Subject("Long polling")]
    public class when_long_polling_for_messages_and_two_messages_are_returned_in_the_stream :
        WithSubject<LongPollReciever>
    {
        static List<MessagePayload> messagePayloads;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;
            
        Establish context = () =>
        {
            messagePayloads = new List<MessagePayload>();
            messagePayload1 = new MessagePayload();
            messagePayload1.SetToAddress(new EndpointAddress("Address1"));
            messagePayload2 = new MessagePayload();
            messagePayload2.SetToAddress(new EndpointAddress("Address2"));

            var address = new EndpointAddress("EndpointAddress");
            Configure<IFormatter>(new BinaryFormatter());
            
            var requestor = new TestWebRequestor(The<IFormatter>());
            requestor.AddMessages(address.GetUrl(), messagePayload1, messagePayload2);
            Configure<IWebRequestor>(requestor);

            Subject = new LongPollReciever(The<IWebRequestor>(), The<IFormatter>());
            Subject.MessageProcessed += payload => messagePayloads.Add(payload);
            Subject.RegisterListeningAddress(address);
        };

        Because of = () => Subject.Poll();

        It should_output_the_first_recieved_message = () =>
            messagePayloads.First().GetToAddress().ShouldEqual(messagePayload1.GetToAddress());

        It should_output_the_second_recieved_message = () =>
             messagePayloads.Last().GetToAddress().ShouldEqual(messagePayload2.GetToAddress());
    }
}