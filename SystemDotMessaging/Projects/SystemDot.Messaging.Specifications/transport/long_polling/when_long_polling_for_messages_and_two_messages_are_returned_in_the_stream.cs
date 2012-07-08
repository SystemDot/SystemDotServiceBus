using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    [Subject("Long polling")]
    public class when_long_polling_for_messages_with_two_messages_are_returned_in_the_stream : WithSubject<LongPollReciever>
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

            Configure<ISerialiser>(new PlatformAgnosticSerialiser());

            var requestor = new TestWebRequestor(The<ISerialiser>(), new FixedPortAddress());
            Configure<IWebRequestor>(requestor); 
            requestor.AddMessages(messagePayload1, messagePayload2);
            
            Subject.MessageProcessed += payload => messagePayloads.Add(payload);
            Subject.RegisterListeningAddress(messagePayload1.GetToAddress());
            Subject.RegisterListeningAddress(messagePayload2.GetToAddress());
        };

        Because of = () => Subject.Poll();

        It should_output_the_first_recieved_message = () =>
            messagePayloads.First().GetToAddress().ShouldEqual(messagePayload1.GetToAddress());

        It should_output_the_second_recieved_message = () =>
             messagePayloads.Last().GetToAddress().ShouldEqual(messagePayload2.GetToAddress());
    }
}