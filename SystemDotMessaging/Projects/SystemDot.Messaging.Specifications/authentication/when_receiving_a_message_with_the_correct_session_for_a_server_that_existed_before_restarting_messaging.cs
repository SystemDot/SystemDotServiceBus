using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;
using SystemDot.Messaging.Authentication;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_the_correct_session_for_a_server_that_existed_before_restarting_messaging 
        : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const int Message = 1;

        static IChangeStore changeStore;
        static MessagePayload payload;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            changeStore = new InMemoryChangeStore(new JsonSerialiser());
            ConfigureAndRegister(changeStore);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(ReceiverServer)
                    .RequiresAuthentication()
                        .AcceptsRequest<TestAuthenticationRequest>()
                        .AuthenticatesOnReply<TestAuthenticationResponse>()
                .OpenChannel(ReceiverChannel).ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            MessagePayload authenticationRequestPayload = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(SenderServer)
                .SetToServer(ReceiverServer);

            AuthenticationSession session = SendMessagesToServer(authenticationRequestPayload).First().GetAuthenticationSession();

            payload = new MessagePayload()
               .SetMessageBody(Message)
               .SetFromChannel("SenderChannel")
               .SetFromServer(SenderServer)
               .SetToChannel(ReceiverChannel)
               .SetToServer(ReceiverServer)
               .SetChannelType(PersistenceUseType.PointToPointSend)
               .Sequenced();

            payload.SetAuthenticationSession(session);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(ReceiverServer)
                    .RequiresAuthentication()
                        .AcceptsRequest<TestAuthenticationRequest>()
                        .AuthenticatesOnReply<TestAuthenticationResponse>()
                .OpenChannel(ReceiverChannel).ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();           
        };

        Because of = () => SendMessagesToServer(payload);

        It should_handle_the_message_in_the_registered_handler = () => handler.HandledMessages.Count.ShouldEqual(1);
    }
}