using System.Collections.Generic;
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
    public class when_long_polling_for_messages_and_the_web_requestor_fails : WithSubject<LongPollReciever>
    {
        static MessagePayload messagePayload;
        static TestTaskStarter starter;
        static EndpointAddress address;
        
        Establish context = () =>
        {
            address = new EndpointAddress("Address1", "TestServer");
            messagePayload = new MessagePayload();
            messagePayload.SetToAddress(address);

            Configure<IWebRequestor>(new FailingWebRequestor()); 

            starter = new TestTaskStarter(2);
            Configure<ITaskStarter>(starter);            
        };

        Because of = () => Subject.StartPolling(address);

        It should_not_fail_but_continue_polling = () => starter.InvocationCount.ShouldEqual(2);
        
    }
}