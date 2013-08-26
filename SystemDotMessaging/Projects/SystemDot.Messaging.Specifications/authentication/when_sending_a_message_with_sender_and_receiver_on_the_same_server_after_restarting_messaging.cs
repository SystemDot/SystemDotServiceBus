using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_sender_and_receiver_on_the_same_server_after_restarting_messaging
        : WithHttpServerConfigurationSubject
    {
        const string Server = "Server";
        const string Channel = "Channel";
        const int Message = 1;

        static IChangeStore changeStore;
        static AuthenticationSession session;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new JsonSerialiser());
            ConfigureAndRegister(changeStore);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(Server)
                        .RequiresAuthentication()
                            .AcceptsRequest<TestAuthenticationRequest>()
                            .AuthenticatesOnReply<TestAuthenticationResponse>()
                    .OpenChannel(Channel).ForPointToPointSendingTo(Channel)
                    .OpenChannel(Channel).ForPointToPointReceiving()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .Initialise();

            MessagePayload authenticationRequestPayload = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(Server)
                .SetToServer(Server);

            session = SendMessagesToServer(authenticationRequestPayload).First().GetAuthenticationSession();

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));

            Configuration.Configure.Messaging()
               .UsingHttpTransport()
                   .AsAServer(Server)
                       .RequiresAuthentication()
                           .AcceptsRequest<TestAuthenticationRequest>()
                           .AuthenticatesOnReply<TestAuthenticationResponse>()
                    .OpenChannel(Channel).ForPointToPointSendingTo(Channel)
                    .OpenChannel(Channel).ForPointToPointReceiving()
                   .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
               .Initialise();

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Send(Message);

        It should_send_the_message_along_with_the_original_session_created_before_the_reset = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetAuthenticationSession().ShouldEqual(session);
    }
}