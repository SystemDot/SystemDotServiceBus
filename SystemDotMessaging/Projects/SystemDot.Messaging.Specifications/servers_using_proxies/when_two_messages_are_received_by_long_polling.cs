using System;
using System.Linq;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.servers_using_proxies
{
    [Subject(SpecificationGroup.Description)]
    public class when_two_messages_are_received_by_long_polling : WithHttpConfigurationSubject
    {
        const string ReceiverName = "ReceiverName";
        const string SenderName = "SenderName";
        const string ServerName = "ServerName";
        const Int64 Message1 = 1;
        const Int64 Message2 = 2;

        static TestTaskStarter taskStarter;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;
        static TestMessageHandler<Int64> handler;

        Establish context = () =>
        {
            taskStarter = new TestTaskStarter(1);
            taskStarter.Pause(); 
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            handler = new TestMessageHandler<Int64>();
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServerUsingAProxy(ServerName)
                .OpenChannel(ReceiverName)
                    .ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            messagePayload1 = new MessagePayload()
                .SetMessageBody(Message1)
                .SetFromChannel(SenderName)
                .SetFromServer(ServerName)
                .SetToChannel(ReceiverName)
                .SetToServer(ServerName)
                .SetToMachineLocal()
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();

            messagePayload2 = new MessagePayload()
                .SetMessageBody(Message2)
                .SetFromChannel(SenderName)
                .SetFromServer(ServerName)
                .SetToChannel(ReceiverName)
                .SetToServer(ServerName)
                .SetToMachineLocal()
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();
        };

        Because of = () =>
        {
            WebRequestor.AddMessages(messagePayload1, messagePayload2);
            taskStarter.UnPause();
        };

        It should_output_the_first_recieved_message = () => handler.HandledMessages.First().ShouldEqual(Message1);

        It should_output_the_second_recieved_message = () => handler.HandledMessages.Last().ShouldEqual(Message2);
    }
}