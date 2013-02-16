using System.Collections.Generic;
using System.Linq;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using Machine.Specifications;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_a_long_poll_request_for_messages_with_two_messages_in_the_queue 
        : WithServerConfigurationSubject
    {
        static MessagePayload sentMessageInQueue1;
        static MessagePayload sentMessageInQueue2;
        static MessagePayload longPollRequest;
        static IEnumerable<MessagePayload> returnedMessages;

        Establish context = () =>
        {
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteServer("RemoteServerInstance");
            
            sentMessageInQueue1 = new MessagePayload();
            sentMessageInQueue1.SetToAddress(GetEndpointAddress("Address2", "TestServer"));

            sentMessageInQueue2 = new MessagePayload();
            sentMessageInQueue2.SetToAddress(GetEndpointAddress("Address2", "TestServer"));

            SendMessagesToServer(sentMessageInQueue1, sentMessageInQueue2);

            longPollRequest = new MessagePayload();
            longPollRequest.SetLongPollRequest(GetEndpointAddress("Address2", "TestServer").ServerPath);
        };

        Because of = () => returnedMessages = SendMessagesToServer(longPollRequest);

        It should_put_the_first_message_in_the_response = () =>
           returnedMessages.First().GetToAddress().ShouldEqual(sentMessageInQueue1.GetToAddress());

        It should_put_the_second_message_in_the_response = () =>
            returnedMessages.Last().GetToAddress().ShouldEqual(sentMessageInQueue2.GetToAddress());
    }
}