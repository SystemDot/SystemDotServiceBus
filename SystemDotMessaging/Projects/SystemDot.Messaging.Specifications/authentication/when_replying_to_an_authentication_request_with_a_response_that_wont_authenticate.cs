using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_an_authentication_request_with_a_response_that_wont_authenticate : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";

        static MessagePayload payload;
        static TestReplyMessageHandler<TestAuthenticationRequest, FailingAuthenticationResponse> handler;
        static IEnumerable<MessagePayload> returnedMessages;

        Establish context = () =>
        {
            handler = new TestReplyMessageHandler<TestAuthenticationRequest, FailingAuthenticationResponse>();

            payload = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer("SenderServer")
                .SetToServer(ReceiverServer);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .RequiresAuthentication()
                .AcceptsRequest<TestAuthenticationRequest>()
                .AuthenticatesOnReply<TestAuthenticationResponse>()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => returnedMessages = SendMessageToServer(payload);

        It should_reply_with_a_response_that_does_not_contain_the_authentication_session_in_the_headers = () =>
            returnedMessages.Single().HasAuthenticationSession().ShouldBeFalse();
    }
}