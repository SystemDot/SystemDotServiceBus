using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_expiry
{
    [Subject(authentication.SpecificationGroup.Description)]
    public class when_replying_to_an_authentication_request_on_a_server_with_session_expiry
       : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const int ExpiryInMinutes = 20;

        static MessagePayload payload;
        static TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse> handler;
        static IEnumerable<MessagePayload> returnedMessages;
        
        Establish context = () =>
        {
            handler = new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>();

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
                        .ExpiresAfter(TimeSpan.FromMinutes(ExpiryInMinutes))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => returnedMessages = SendMessagesToServer(payload);

        It should_reply_with_the_specified_authentication_response_containing_the_new_authentication_session_with_the_specified_expiry = () =>
            returnedMessages.Single().GetAuthenticationSession().ExpiresOn.ShouldEqual(SystemTime.GetCurrentDate().AddMinutes(ExpiryInMinutes));
    }

    
}