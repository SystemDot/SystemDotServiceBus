using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote
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

        Because of = () => Subject.RegisterAddress(address);

        It should_not_fail_but_continue_polling = () => starter.InvocationCount.ShouldEqual(2);
        
    }
}