using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    [Subject("Long polling")]
    public class when_long_polling_for_messages_with_two_messages_are_returned_in_the_stream : WithSubject<LongPollReciever>
    {
        const string ServerName = "ServerName";
        static List<MessagePayload> messagePayloads;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;
        static EndpointAddress endpointAddress;

        Establish context = () =>
        {
            messagePayloads = new List<MessagePayload>();
            messagePayload1 = new MessagePayload();
            messagePayload2 = new MessagePayload();
            endpointAddress = new EndpointAddress("Address", ServerName);
            
            messagePayload1.SetToAddress(endpointAddress);
            messagePayload2.SetToAddress(endpointAddress);

            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            
            var requestor = new TestWebRequestor(The<ISerialiser>(), new FixedPortAddress(ServerName));
            Configure<IWebRequestor>(requestor);
            requestor.AddMessages(messagePayload1, messagePayload2);

            var starter = new TestTaskStarter(1);
            Configure<ITaskStarter>(starter);

            Subject.MessageProcessed += payload => messagePayloads.Add(payload);            
        };

        Because of = () => Subject.RegisterAddress(endpointAddress);

        It should_output_the_first_recieved_message = () =>
            messagePayloads.First().GetToAddress().ShouldEqual(messagePayload1.GetToAddress());

        It should_output_the_second_recieved_message = () =>
             messagePayloads.Last().GetToAddress().ShouldEqual(messagePayload2.GetToAddress());
    }
}