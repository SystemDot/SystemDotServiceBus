using System.Collections.Generic;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    [Subject("Long polling")]
    public class when_a_response_to_a_long_poll_request_has_been_processed : WithSubject<LongPollReciever>
    {
        const string ServerName = "ServerName";
        static List<MessagePayload> messagePayloads;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            messagePayloads = new List<MessagePayload>();
            messagePayload = new MessagePayload();
            messagePayload.SetToAddress(new EndpointAddress("Address1", ServerName));
            
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());

            var requestor = new TestWebRequestor(The<ISerialiser>(), new FixedPortAddress(ServerName));
            Configure<IWebRequestor>(requestor);
            requestor.AddMessages(messagePayload);

            var starter = new TestTaskStarter(2);
            Configure<ITaskStarter>(starter);

            Subject.MessageProcessed += payload => messagePayloads.Add(payload);
        };

        Because of = () => Subject.StartPolling(messagePayload.GetToAddress());

        It should_poll_again = () => messagePayloads.Count.ShouldEqual(2);
    }
}